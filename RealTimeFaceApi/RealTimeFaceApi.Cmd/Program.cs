using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using OpenCvSharp;
using RealTimeFaceApi.Core.Data;
using RealTimeFaceApi.Core.Filters;
using RealTimeFaceApi.Core.Trackers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using static System.Net.Mime.MediaTypeNames;
using Google.Cloud.Vision.V1;
//using WebTracNghiemOnline;

namespace RealTimeFaceApi.Cmd
{

    public class Program
    {
        private static string filePath = @"D:\\Check.txt";
        private static int check = 1;
        // TODO: Add Face API subscription key.
        private static string FaceSubscriptionKey = "";

        // TODO: Add face group ID.
        private static string FaceGroupId = "";

        private static readonly Scalar _faceColorBrush = new Scalar(0, 0, 255);
        private static FaceClient _faceClient;
        private static Task _faceRecognitionTask = null;
        private static IList<Person> _cachedIdentities = null;
        private static void sqlCon(int x)
        {
            var datasource = @"DESKTOP-2FLS3J3\PHISQL";//your server
            var database = "TRACNGHIEM_ONLINE"; //your database name
            var username = "sa"; //username of server to connect
            var password = "tuanphi1609"; //password

            //your connection string 
            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                        //+ database + ";Persist Security Info=True;";
                        + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            //create instanace of database connection
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            //create a new SQL Query using StringBuilder
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("INSERT INTO BangTam VALUES ");
            strBuilder.Append("("+x.ToString()+")");

            string sqlQuery = strBuilder.ToString();
            using (SqlCommand command = new SqlCommand(sqlQuery, conn)) //pass SQL query created above and connection
            {
                command.ExecuteNonQuery(); //execute the Query
                Console.WriteLine("Query Executed.");
            }
        }

        public static void Main(string[] args)
        {


            //// Instantiates a client
            //var client = ImageAnnotatorClient.Create();
            //// Load the image file into memory
            //var image = Image.FromFile("wakeupcat.jpg");
            //// Performs label detection on the image file
            //var response = client.DetectLabels(image);
            //foreach (var annotation in response)
            //{
            //    if (annotation.Description != null)
            //        Console.WriteLine(annotation.Description);
            //}

            //GetCheck gt = new GetCheck();


            _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(FaceSubscriptionKey))
            {
                Endpoint = "https://westus.api.cognitive.microsoft.com"
            };

            if (string.IsNullOrWhiteSpace(FaceSubscriptionKey))
            {
                var defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Face API configuration is not configured");
                Console.ForegroundColor = defaultColor;
            }

            string filename = args.FirstOrDefault();
            Run(filename);
        }
        

        public static void GhiFile(string path, string content)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);
            //if (System.IO.File.Exists("D:\\Check2.txt"))
            //{
            //    System.IO.File.Delete("D:\\Check2.txt");
            //}

            //System.IO.File.Copy(Path.Combine("D:\\", "Check.txt"), Path.Combine("D:\\", "Check2.txt"));
            sWriter.WriteLine(content);
            sWriter.Flush();
            fs.Close();
            fs.Dispose();
        }
        
        public static string ReturnPath()
        {
            string path = Environment.CurrentDirectory;
            return path;
        }
        public static int GetCheck(int n)
        {
            if (n > 0)
            {
                return 1;
            }
            return 0;
        }
        private static void Run(string filename)
        {
            int timePerFrame;
            VideoCapture capture;
            if (!string.IsNullOrWhiteSpace(filename) && File.Exists(filename))
            {
                // If filename exists, use that as a source of video.
                capture = InitializeVideoCapture(filename);

                // Allow just enough time to paint the frame on the window.
                timePerFrame = 1;
            }
            else
            {
                // Otherwise use the webcam.
                capture = InitializeCapture(0);

                // Time required to wait until next frame.
                timePerFrame = (int)Math.Round(3053 / capture.Fps);
            }

            // Input was not initialized.
            if (capture == null)
            {
                Console.ReadKey();
                return;
            }

            // Initialize face detection algorithm.
            CascadeClassifier haarCascade = InitializeFaceClassifier();

            // List of simple face filtering algorithms.
            var filtering = new SimpleFaceFiltering(new IFaceFilter[]
            {
                new TooSmallFacesFilter(20, 20)
            });

            // List of simple face tracking algorithms.
            var trackingChanges = new SimpleFaceTracking(new IFaceTrackingChanged[]
            {
                new TrackNumberOfFaces(),
                new TrackDistanceOfFaces { Threshold = 2000 }
            });

            // Open a new window via OpenCV.
            using (Window window = new Window("capture"))
            {
                using (Mat image = new Mat())
                {
                    while (true)
                    {
                        // Get current frame.
                        capture.Read(image);
                        if (image.Empty())
                            continue;

                        // Detect faces
                        var faces = DetectFaces(haarCascade, image);

                        // Filter faces
                        var state = faces.ToImageState();
                        state = filtering.FilterFaces(state);

                        // Determine change
                        var hasChange = trackingChanges.ShouldUpdateRecognition(state);

                        if (hasChange)
                        {
                            //Console.WriteLine("Changes detected...");
                            switch (check)
                            {
                                case 0:
                                    {
                                        check = 1;
                                        //GhiFile(filePath, "1");
                                        sqlCon(check);
                                        //Console.WriteLine(GetCheck(check));
                                        //Console.WriteLine(check);
                                        break;
                                    }
                                case 1:
                                    {
                                        check = 0;
                                        //GhiFile(filePath, "0");
                                        sqlCon(check);
                                        //Console.WriteLine(GetCheck(check));
                                        //Console.WriteLine(check);
                                        break;
                                    }
                            }

                            // Identify faces if changed and previous identification finished.
                            if (_faceRecognitionTask == null && !string.IsNullOrWhiteSpace(FaceSubscriptionKey))
                            {
                                _faceRecognitionTask = StartRecognizing(image);
                            }
                        }

                        using (var renderedFaces = RenderFaces(state, image))
                        {
                            // Update popup window.
                            window.ShowImage(renderedFaces);
                        }

                        // Wait for next frame and allow Window to be repainted.
                        Cv2.WaitKey(timePerFrame);
                    }
                }
            }
        }

        /// <summary>
        /// Use Microsoft Cognitive Services to recognize their faces.
        /// </summary>
        /// <param name="image">Video or web cam frame.</param>
        private static async Task StartRecognizing(Mat image)
        {
            try
            {
                // TODO: Optimize by cropping the image to only the face (with 10-30% padding) to have <50kB upload.
                var stream = image.ToMemoryStream();

                Console.WriteLine(DateTime.Now + ": Sending " + (stream.Length / 1024) + "kB to recognize face.");

                Stopwatch stopwatch = Stopwatch.StartNew();
                var detectedFaces = await _faceClient.Face.DetectWithStreamAsync(stream, true, true);
                var faceIds = detectedFaces.Where(f => f.FaceId.HasValue).Select(f => f.FaceId.Value).ToList();
                Console.WriteLine(DateTime.Now + ": Found " + faceIds.Count + " faces in " + stopwatch.ElapsedMilliseconds + "ms.");

                stopwatch.Stop();

                if (faceIds.Any())
                {
                    stopwatch.Restart();

                    var potentialUsers = await _faceClient.Face.IdentifyAsync(faceIds, FaceGroupId);

                    stopwatch.Stop();

                    Console.WriteLine(DateTime.Now + ": Recognized " + potentialUsers.Count + " candidates in " + stopwatch.ElapsedMilliseconds + "ms.");
                    foreach (var candidate in potentialUsers.Select(u => u.Candidates.FirstOrDefault()))
                    {
                        var candidateName = await GetCandidateName(candidate?.PersonId);
                        Console.WriteLine($"{DateTime.Now}: {candidateName} ({candidate?.PersonId})");
                    }
                }
                else
                {
                    Console.WriteLine(DateTime.Now + $": No clear shot on the faces.");
                }
            }
            catch (APIErrorException apiError)
            {
                Console.WriteLine(DateTime.Now + ": Cognitive service error: " + apiError?.Body?.Error?.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now + ": Getting identity failed: " + e.ToString());
            }

            _faceRecognitionTask = null;
        }

        private async static Task<string> GetCandidateName(Guid? personId)
        {
            if (!personId.HasValue)
            {
                return "No Person ID";
            }

            if (_cachedIdentities == null)
            {
                _cachedIdentities = await _faceClient.PersonGroupPerson.ListAsync(FaceGroupId);
            }

            return _cachedIdentities
                ?.Where(i => i.PersonId == personId)
                .Select(i => i.Name)
                .FirstOrDefault()
                ?? "Candidate not found";
        }

        /// <summary>
        /// Initialize classifier used for offline face detection.
        /// </summary>
        private static CascadeClassifier InitializeFaceClassifier()
        {
            return new CascadeClassifier("Data/haarcascade_frontalface_alt.xml");
        }

        /// <summary>
        /// Initialize web cam capture.
        /// </summary>
        /// <returns>Returns web cam capture.</returns>
        private static VideoCapture InitializeCapture(int cameraIndex = 0)
        {
            VideoCapture capture = new VideoCapture();
            capture.Open(cameraIndex, VideoCaptureAPIs.ANY);

            if (!capture.IsOpened())
            {
                Console.WriteLine("Unable to open capture.");
                return null;
            }

            return capture;
        }

        /// <summary>
        /// Initializes video capture for video files.
        /// </summary>
        /// <param name="file">Path to a video.</param>
        /// <returns>Return video file capture.</returns>
        private static VideoCapture InitializeVideoCapture(string file)
        {
            var capture = new VideoCapture(file);
            if (!capture.IsOpened())
            {
                Console.WriteLine("Unable to open video file {0}.", file);
                return null;
            }

            return capture;
        }

        /// <summary>
        /// Use OpenCV Cascade classifier to do offline face detection.
        /// </summary>
        /// <param name="cascadeClassifier">OpenCV cascade classifier.</param>
        /// <param name="image">Web cam or video frame.</param>
        /// <returns>Return list of faces as rectangles.</returns>
        private static Rect[] DetectFaces(CascadeClassifier cascadeClassifier, Mat image)
        {
            return cascadeClassifier
                .DetectMultiScale(
                    image,
                    1.08,
                    2,
                    HaarDetectionType.ScaleImage,
                    new Size(60, 60));
        }

        /// <summary>
        /// Render detected faces via OpenCV.
        /// </summary>
        /// <param name="state">Current frame state.</param>
        /// <param name="image">Web cam or video frame.</param>
        /// <returns>Returns new image frame.</returns>
        private static Mat RenderFaces(FrameState state, Mat image)
        {
            Mat result = image.Clone();
            Cv2.CvtColor(image, image, ColorConversionCodes.BGR2GRAY);

            // Render all detected faces
            foreach (var face in state.Faces)
            {
                var center = new Point
                {
                    X = face.Center.X,
                    Y = face.Center.Y
                };
                var axes = new Size
                {
                    Width = (int)(face.Size.Width * 0.5) + 10,
                    Height = (int)(face.Size.Height * 0.5) + 10
                };

                Cv2.Ellipse(result, center, axes, 0, 0, 360, _faceColorBrush, 4);
            }

            return result;
        }
    }
}
