using DPUruNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using UareUSampleCSharp;
using System.Transactions;
using WebSocketSharp;




namespace project
{
    public partial class FormDbEnrollment : Form
    {
        List<DPUruNet.Fmd> preenrollmentFmds;
        int count = 0;
        public FormDbEnrollment()
        {
            InitializeComponent();
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





        private Reader _reader;


        #region Click Event Handlers
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
        /// <summary>
        /// Function to capture a finger. Always get status first and calibrate or wait if necessary.  Always check status and capture errors.
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Cancel the capture and then close the reader.
        /// </summary>
        /// <param name="OnCaptured">Delegate to unhook as handler of the On_Captured event </param>
        public void CancelCaptureAndCloseReader(Reader.CaptureCallback OnCaptured)
        {
            using (Tracer tracer = new Tracer("Form_Main::CancelCaptureAndCloseReader"))
            {
                if (currentReader != null)
                {
                    currentReader.CancelCapture();

                    // Dispose of reader handle and unhook reader events.
                    currentReader.Dispose();

                    if (reset)
                    {
                        CurrentReader = null;
                    }
                }
            }
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

        private UareUSampleCSharp.Capture _capture;
        private WebSocket client;
        const string host = "wss://echo.websocket.org";



        private void FormDbEnrollment_Load(object sender, EventArgs e)
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
                //this.Close();
            }

            if (!StartCaptureAsync(this.OnCaptured))
            {
                // this.Close();
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
        /// <summary>
        /// Open a device and check result for errors.
        /// </summary>
        /// <returns>Returns true if successful; false if unsuccessful</returns>
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

        /// <summary>
        /// Handler for when a fingerprint is captured.
        /// </summary>
        /// <param name="captureResult">contains info and data on the fingerprint capture</param>
        private const int PROBABILITY_ONE = 0x7fffffff;
        private Fmd firstFinger;

        DataResult<Fmd> resultEnrollment;

        /// <summary>
        /// Handler for when a fingerprint is captured.
        /// </summary>
        /// <param name="captureResult">contains info and data on the fingerprint capture</param>
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
                //Enrollment Code
                try
                {
                    count++;
                    // Check capture quality and throw an error if bad.
                    DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);

                    MessageBox.Show("A finger was captured.  \r\nCount:  " + (count));

                    if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        Reset = true;
                        throw new Exception(resultConversion.ResultCode.ToString());
                    }

                    preenrollmentFmds.Add(resultConversion.Data);

                    if (count >= 4)
                    {
                        resultEnrollment = DPUruNet.Enrollment.CreateEnrollmentFmd(Constants.Formats.Fmd.ANSI, preenrollmentFmds);

                        if (resultEnrollment.ResultCode == Constants.ResultCode.DP_SUCCESS)
                        {
                            preenrollmentFmds.Clear();
                            count = 0;
                            //obj_bal_ForAll.BAL_StoreCustomerFPData("tbl_Finger", txtLedgerId.Text, Fmd.SerializeXml(resultEnrollment.Data));
                            MessageBox.Show("Customer Finger Print was successfully enrolled.");
                            return;
                        }
                        else if (resultEnrollment.ResultCode == Constants.ResultCode.DP_ENROLLMENT_INVALID_SET)
                        {
                            SendMessage(Action.SendMessage, "Enrollment was unsuccessful.  Please try again.");
                            preenrollmentFmds.Clear();
                            count = 0;
                            return;
                        }
                    }
                    MessageBox.Show(" Place the same finger on Reader");
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
            }
        }

        /// <summary>
        /// Create a bitmap from raw data in row/column format.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
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


        private void FormDbEnrollment_FormClosing(object sender, FormClosedEventArgs e)
        {
            CancelCaptureAndCloseReader(this.OnCaptured);
        }
        private void button1_Click(object sender, EventArgs e)
        {


            string constring = "Server=localhost;Port=3307;Database=fingerspotdb;Uid=root;Pwd=;";
            string Query = "INSERT INTO dbo (LedgerID, CustomerFinger) VALUES ('" + this.txtLedgerId.Text + "','" + Fmd.SerializeXml(resultEnrollment.Data) + "');";
            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;
            if (resultEnrollment != null)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (!string.IsNullOrEmpty(this.txtLedgerId.Text + Fmd.SerializeXml(resultEnrollment.Data)))
                client.Send(this.txtLedgerId.Text + Fmd.SerializeXml(resultEnrollment.Data));
            
        }

        private void Conn_btn_Click(object sender, EventArgs e)
        {
            client.Connect();
        }

        private void Disconn_btn_Click(object sender, EventArgs e)
        {
            client.Close();
        }
    }
}
#endregion