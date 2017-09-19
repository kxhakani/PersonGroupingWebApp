using System;
using VideoFrameAnalyzer;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System.Threading.Tasks;
using OpenCvSharp.Extensions;
using System.Linq;
using System.Data.SqlClient;
using System.Web.UI;
using System.Configuration;
using System.Web.Services;
using System.Web.Mvc;
using System.IO;

namespace PersonGroupWebsite
{
    /// <summary>
    /// Interaction logic for videoLogin.xaml
    /// </summary>
    public partial class videoLogin : Page
    {
        private static string ServiceKey = ConfigurationManager.AppSettings["FaceServiceKey"];

        // Create grabber, with analysis type Face[]. 
        FrameGrabber<Face[]> grabber = new FrameGrabber<Face[]>();

        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient(ServiceKey, "https://westus.api.cognitive.microsoft.com/face/v1.0");

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Call from javascript
        [WebMethod]
        public void Login()
        {
            //Take Still frame from video
            var stream = Request.InputStream;
            string dump;
            using (var reader = new StreamReader(stream))
            {
                dump = reader.ReadToEnd();
                DateTime nm = DateTime.Now;
                string date = nm.ToString("yyyymmddMMss");
                var path = Server.MapPath("/WebImages/" + date + "test.jpg");
                File.WriteAllBytes(path, String_To_Bytes2(dump));
                Session["val"] = date + "test.jpg";
                imgCapture.Src = path;
            }

            
            //Pass it to face detect/identify


            //Verify identity

        }

        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;
            byte[] bytes = new byte[numBytes];
            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }
            return bytes;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        //Find faces in streaming video
        /*public void testFaces(Guid person)
        {
            grabber = new FrameGrabber<Face[]>();

            //Set up our Face API call.
            grabber.AnalysisFunction = async frame => await faceServiceClient.DetectAsync(frame.Image.ToMemoryStream(".jpg"));

            //Set up a listener for new frames
            grabber.NewFrameProvided += (s, e) =>
            {
                //videoCapture.Source = e.Frame.Image.ToBitmapSource();
            };

            // Set up a listener for when we receive a new result from an API call. 
            grabber.NewResultAvailable += (s, e) =>
            {
                if (e.Analysis != null)
                    Console.WriteLine("New result received for frame acquired at {0}. {1} faces detected", e.Frame.Metadata.Timestamp, e.Analysis.Length);

                this.Dispatcher.BeginInvoke((Action)(async () =>
                {
                    BitmapSource image = e.Frame.Image.ToBitmapSource();
                    Face[] results = e.Analysis;
                    if (results != null)
                    {
                        VerifyResult auth = null;

                        DrawingVisual visual = new DrawingVisual();
                        DrawingContext drawingContext = visual.RenderOpen();
                        drawingContext.DrawImage(image, new System.Windows.Rect(0, 0, image.Width, image.Height));

                        if (results.Count() == 1)
                        {
                            Face face = results[0];

                            auth = await authenticateUser(face, person);

                            System.Windows.Rect faceRectangle = new System.Windows.Rect(
                            face.FaceRectangle.Left, face.FaceRectangle.Top,
                            face.FaceRectangle.Width, face.FaceRectangle.Height);

                            drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Red, 3), faceRectangle);
                        }

                        drawingContext.Close();

                        RenderTargetBitmap render = new RenderTargetBitmap(
                            image.PixelWidth, image.PixelHeight,
                            image.DpiX, image.DpiY, PixelFormats.Pbgra32);

                        render.Render(visual);
                        faceDisplay.Source = render;

                        if (auth != null && auth.IsIdentical)
                        {
                            await grabber.StopProcessingAsync();
                            Helpers.MessageBox.Show(this, "User has been Authenticated");
                        }
                    }

                }));
            };

            // Tell grabber to call the Face API every 3 seconds.
            grabber.TriggerAnalysisOnInterval(TimeSpan.FromMilliseconds(3000));

            // Start running.
            if (grabber.GetNumCameras() > 0)
                grabber.StartProcessingCameraAsync(lstCameras.SelectedIndex).Wait();
            else
                Helpers.MessageBox.Show(this, "There are no cameras to start");
        }

        private async Task<VerifyResult> authenticateUser(Face face, Guid person)
        {
            VerifyResult auth = null;
            string loginGroup = cmboLoginGroup.Text;

            auth = await faceServiceClient.VerifyAsync(face.FaceId, loginGroup, person);

            return auth;
        }

        private Guid getUser()
        {
            string loginGroup = cmboLoginGroup.Text;
            string userName = txtUserName.Text;
            string PersonId = "";
            bool connectionError = false;

            string connectionString = null;
            connectionString = "Server=tcp:sdc-poc-mto-511-test.database.windows.net,1433;Initial Catalog=faceapi-userdb;Persist Security Info=False;User ID=sdcpoc511;Password=Testing511poc;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    String query = "SELECT * FROM dbo.UserInformation WHERE userName=@userName and PersonGroupId=@personGroup;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userName", userName);
                        command.Parameters.AddWithValue("@personGroup", loginGroup);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PersonId = reader["PersonId"].ToString();
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                connectionError = true;
                Helpers.MessageBox.Show(this, "Unable to connect to the server. Please try again later.");
            }

            Guid userId = Guid.Empty;
            bool isValid = Guid.TryParse(PersonId, out userId);

            if (!isValid && !connectionError)
                Helpers.MessageBox.Show(this, "User could not be found");

            return userId;
        }
        private async void PopulateComboBox()
        {
            string[] groups;

            PersonGroup[] personGroups = await faceServiceClient.ListPersonGroupsAsync();

            groups = personGroups.Select(groupId => groupId.PersonGroupId).ToArray();

            cmboLoginGroup.ItemsSource = groups;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            StartupWindow startupWindow = new StartupWindow();
            startupWindow.Show();
            this.Close();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Guid user = getUser();

            if (!user.Equals(Guid.Empty))
                testFaces(user);

        }

        private async void btnStop_Click(object sender, RoutedEventArgs e)
        {
            await grabber.StopProcessingAsync();
        }

        private void cmboLoginGroup_DropDownOpened(object sender, EventArgs e)
        {
            PopulateComboBox();
        }

        private void lstCameras_Loaded(object sender, RoutedEventArgs e)
        {
            int numCameras = grabber.GetNumCameras();

            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = Enumerable.Range(0, numCameras).Select(i => string.Format("Camera {0}", i + 1));
            comboBox.SelectedIndex = 0;
        }*/
    }
}
