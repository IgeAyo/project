using DPUruNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UareUSampleCSharp;
using WebSocketSharp;


namespace project
{
    public partial class FormDbVerification : Form

    {

        string constring = "Server=localhost;Port=3307;Database=fingerspotdb;Uid=root;Pwd=;";

        public FormDbVerification()
        {
            InitializeComponent();
        }
        private Reader currentReader;
        public Reader CurrentReader
        {
            get { return currentReader; }
            set
            {
                currentReader = value;
                SendMessage(Action.UpdateReaderState, value);
            }
        }
        private ReaderCollection _readers;
        private void LoadScanners()
        {
            comboBox1.Text = string.Empty;
            comboBox1.Items.Clear();
            comboBox1.SelectedIndex = -1;

            try
            {
                _readers = ReaderCollection.GetReaders();

                foreach (Reader Reader in _readers)
                {
                    comboBox1.Items.Add(Reader.Description.Name);
                }

                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                    //btnCaps.Enabled = true;
                    //btnSelect.Enabled = true;
                }
                else
                {
                    //btnSelect.Enabled = false;
                    //btnCaps.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //message box:
                String text = ex.Message;
                text += "\r\n\r\nPlease check if DigitalPersona service has been started";
                String caption = "Cannot access readers";
                MessageBox.Show(text, caption);
            }
        }
        private const int PROBABILITY_ONE = 0x7fffffff;
        private Fmd firstFinger;
        int count = 0;
        DataResult<Fmd> resultEnrollment;
        List<Fmd> preenrollmentFmds;
        public bool OpenReader()
        {
            using (Tracer tracer = new Tracer("Form_Main::OpenReader"))
            {
                reset = false;
                Constants.ResultCode result = Constants.ResultCode.DP_DEVICE_FAILURE;
                if (currentReader == null)
                {
                    MessageBox.Show("currentReader is null");
                    reset = true;
                    return false;
                }

                // Open reader
                result = currentReader.Open(Constants.CapturePriority.DP_PRIORITY_COOPERATIVE);

                if (result != Constants.ResultCode.DP_SUCCESS)
                {
                    MessageBox.Show("Error:  " + result);
                    reset = true;
                    return false;
                }

                return true;
            }
        }
        /// <summary>
        /// Check quality of the resulting capture.
        /// </summary>
        public bool CheckCaptureResult(CaptureResult captureResult)
        {
            using (Tracer tracer = new Tracer("Form_Main::CheckCaptureResult"))
            {
                if (captureResult.Data == null || captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    if (captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        reset = true;
                        throw new Exception(captureResult.ResultCode.ToString());
                    }

                    // Send message if quality shows fake finger
                    if ((captureResult.Quality != Constants.CaptureQuality.DP_QUALITY_CANCELED))
                    {
                        throw new Exception("Quality - " + captureResult.Quality);
                    }
                    return false;
                }

                return true;
            }
        }
        private ReaderSelection _readerSelection;

        /// <summary>
        /// Hookup capture handler and start capture.
        /// </summary>
        /// <param name="OnCaptured">Delegate to hookup as handler of the On_Captured event</param>
        /// <returns>Returns true if successful; false if unsuccessful</returns>
        public bool StartCaptureAsync(Reader.CaptureCallback OnCaptured)
        {
            using (Tracer tracer = new Tracer("Form_Main::StartCaptureAsync"))
            {
                // Check if currentReader is null before using it
                if (currentReader == null)
                {
                    MessageBox.Show("currentReader is null");
                    return false;
                }

                // Activate capture handler
                currentReader.On_Captured += OnCaptured;  // No need for new Reader.CaptureCallback here

                // Call capture
                if (!CaptureFingerAsync())
                {
                    return false;
                }

                return true;
            }
        }
        public void GetStatus()
        {
            using (Tracer tracer = new Tracer("Form_Main::GetStatus"))
            {
                Constants.ResultCode result = currentReader.GetStatus();

                if ((result != Constants.ResultCode.DP_SUCCESS))
                {
                    reset = true;
                    throw new Exception("" + result);
                }

                if ((currentReader.Status.Status == Constants.ReaderStatuses.DP_STATUS_BUSY))
                {
                    Thread.Sleep(50);
                }
                else if ((currentReader.Status.Status == Constants.ReaderStatuses.DP_STATUS_NEED_CALIBRATION))
                {
                    currentReader.Calibrate();
                }
                else if ((currentReader.Status.Status != Constants.ReaderStatuses.DP_STATUS_READY))
                {
                    throw new Exception("Reader Status - " + currentReader.Status.Status);
                }
            }
        }
        public bool CaptureFingerAsync()
        {
            using (Tracer tracer = new Tracer("Form_Main::CaptureFingerAsync"))
            {
                try
                {
                    GetStatus();

                    Constants.ResultCode captureResult = currentReader.CaptureAsync(Constants.Formats.Fid.ANSI, Constants.CaptureProcessing.DP_IMG_PROC_DEFAULT, currentReader.Capabilities.Resolutions[0]);
                    if (captureResult != Constants.ResultCode.DP_SUCCESS)
                    {
                        reset = true;
                        throw new Exception("" + captureResult);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:  " + ex.Message);
                    return false;
                }
            }
        }
        public Bitmap CreateBitmap(byte[] bytes, int width, int height)
        {
            byte[] rgbBytes = new byte[bytes.Length * 3];

            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                rgbBytes[(i * 3)] = bytes[i];
                rgbBytes[(i * 3) + 1] = bytes[i];
                rgbBytes[(i * 3) + 2] = bytes[i];
            }
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            for (int i = 0; i <= bmp.Height - 1; i++)
            {
                IntPtr p = new IntPtr(data.Scan0.ToInt64() + data.Stride * i);
                System.Runtime.InteropServices.Marshal.Copy(rgbBytes, i * bmp.Width * 3, p, bmp.Width * 3);
            }

            bmp.UnlockBits(data);

            return bmp;
        }
        public void OnCaptured(CaptureResult captureResult)

        {
            try
            {
                // Check capture quality and throw an error if bad.
                if (!CheckCaptureResult(captureResult)) return;

                // Create bitmap
                foreach (DPUruNet.Fid.Fiv fiv in captureResult.Data.Views)
                {
                    SendMessage(Action.SendBitmap, CreateBitmap(fiv.RawImage, fiv.Width, fiv.Height));
                }
                //Verification Code
                try
                {
                    //check the capture quality and throw an error if bad
                    if (!CheckCaptureResult(captureResult)) return;
                    SendMessage(Action.SendMessage, "A finger has been captured");
                    DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);
                    if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        if (resultConversion.ResultCode != Constants.ResultCode.DP_TOO_SMALL_AREA)
                        {
                            Reset = true;
                        }
                        throw new Exception(resultConversion.ResultCode.ToString());
                    }
                    firstFinger = resultConversion.Data;
                    MySqlConnection conn = new MySqlConnection(constring);
                    conn.Close();
                    conn.Open();
                    MySqlDataAdapter cmd = new MySqlDataAdapter("Select * from tblFinger", conn);
                    DataTable dt = new DataTable();
                    cmd.Fill(dt);
                    conn.Close();

                    List<string> lstledgerIds = new List<string>();
                    count = 0;
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lstledgerIds.Add(dt.Rows[i]["LedgerId"].ToString());
                            Fmd val = Fmd.DeserializeXml(dt.Rows[i]["CustomerFinger"].ToString());
                            CompareResult compare = Comparison.Compare(firstFinger, 0, val, 0);
                            if (compare.ResultCode != Constants.ResultCode.DP_SUCCESS)
                            {
                                Reset = true;
                                throw new Exception(compare.ResultCode.ToString());
                            }
                            if (Convert.ToDouble(compare.Score.ToString()) == 0)
                            {
                                MessageBox.Show("Ledger Id is : " + lstledgerIds[i].ToString());
                                count++;
                                break;
                            }

                        }
                        if (count == 0)
                        {
                            SendMessage(Action.SendMessage, "Fingerprint not registered.");
                        }
                    }

                }
                catch (Exception ex)
                {
                    // Send error message, then close form
                    SendMessage(Action.SendMessage, "Error:  " + ex.Message);
                }

            }

            catch (Exception ex)
            {
                // Send error message, then close form
                SendMessage(Action.SendMessage, "Error:  " + ex.Message);
                DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);
                if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    if (resultConversion.ResultCode != Constants.ResultCode.DP_TOO_SMALL_AREA)
                    {
                        Reset = true;
                    }
                    throw new Exception(resultConversion.ResultCode.ToString());
                }
            }
        }
        /// <summary>
        /// Holds fmds enrolled by the enrollment GUI.
        /// </summary>
        public Dictionary<int, DPUruNet.Fmd> Fmds
        {
            get { return fmds; }
            set { fmds = value; }
        }
        private Dictionary<int, DPUruNet.Fmd> fmds = new Dictionary<int, DPUruNet.Fmd>();

        /// <summary>
        /// Reset the UI causing the user to reselect a reader.
        /// </summary>
        public bool Reset
        {
            get { return reset; }
            set { reset = value; }
        }
        private bool reset;

        private enum Action
        {
            UpdateReaderState,
            SendBitmap,
            SendMessage

        }
        private delegate void SendMessageCallback(Action state, object payload);

        private void SendMessage(Action action, object payload)
        {
            try
            {
                if (this.pbFingerprint.InvokeRequired)
                {
                    SendMessageCallback d = new SendMessageCallback(SendMessage);
                    this.Invoke(d, new object[] { action, payload });
                }
                else
                {
                    switch (action)
                    {
                        case Action.SendMessage:
                            MessageBox.Show((string)payload);
                            break;
                        case Action.SendBitmap:
                            pbFingerprint.Image = (Bitmap)payload;
                            pbFingerprint.Refresh();
                            break;

                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private WebSocket client;
        const string host = "wss://echo.websocket.org";

        private void FormDbVerification_Load(object sender, EventArgs e)
        {

            // Reset variables
            LoadScanners();
            firstFinger = null;
            resultEnrollment = null;
            preenrollmentFmds = new List<Fmd>();
            pbFingerprint.Image = null;
            if (currentReader != null)
            {
                currentReader.Dispose();
                currentReader = null;
            }
            currentReader = _readers[comboBox1.SelectedIndex];

            if (!OpenReader())
            {
                this.Close();
            }

            if (!StartCaptureAsync(this.OnCaptured))
            {
                this.Close();
            }
            //Websocket client Code
            client = new WebSocket(host);
            client.OnOpen += (ss, ee) =>
               listBox1.Items.Add(string.Format("connected to {0} successfully", host));
            client.OnError += (ss, ee) =>
      listBox1.Items.Add("Error: " + ee.Message);
            client.OnMessage += (ss, ee) =>
                listBox1.Items.Add("Echo: " + ee.Data);
            client.OnClose += (ss, ee) =>
                listBox1.Items.Add(string.Format("Disconnected with {0}", host));
        }
    }
}
