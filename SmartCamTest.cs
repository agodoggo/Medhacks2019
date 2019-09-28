// Initialize logging to output all messages with severity level INFO or higher to the console
    bool bWriteToConsole = true;
    bool bWriteToFile = false;
    Framosai.FAIM.Api.initialise_logging(Framosai.FAIM.LogLevel.FAIM_LL_ERROR, bWriteToConsole, bWriteToFile);
    // Query the FAIM_SDK variable to access the installed models, test images, and the activation key
var faimDir = Environment.GetEnvironmentVariable("FAIM_SDK", EnvironmentVariableTarget.Machine);
Framosai.FAIM.SkeletonTracking.Api skeletontrackingApi = new Framosai.FAIM.SkeletonTracking.Api(faimDir + "\\activation_key.txt");                
// Initialise FAIM DNN framework with the required deep learning model and the target compute device
// FP32 model is necessary for the CPU device
skeletontrackingApi.LoadModel(Framosai.FAIM.Target_Compute_Device.FAIM_TCD_CPU, faimDir + "\\bin\\models\\skeleton-tracking\\FP32\\skeleton-tracking.faim");        
// Wait for images from RealSense
var frames = pipeline.WaitForFrames();
// Get the color frame from the intel realsense
var colorFrame = frames.ColorFrame.DisposeWith(frames);
// Convert the input image to a bitmap
Bitmap inputImage = FrameToBitmap(colorFrame);

// Run the inference on the preprocessed image
List<SkeletonKeypoints> skeletonKeypoints;
skeletontrackingApi.RunInference(ref inputImage, networkHeight, out skeletonKeypoints);     
// create the alignment object to the color stream
var align = new Align(Intel.RealSense.Stream.Color);
// Wait for images from RealSense
var frames = pipeline.WaitForFrames()
// Align all frames to the color frame
var processedFrames = frames.ApplyFilter(align).DisposeWith(frames);
// Get the aligned depth frame
var depthFrame = processedFrames.DepthFrame.DisposeWith(frames);
List<SkeletonKeypoints> skeletons = ...; //computed earlier
foreach (var skeleton in skeletons)
    {
        foreach (Coordinate coordinate in skeleton.listJoints)
        {
            // retrieve depth values in local neighbourhood of coordinate
            float[,] depthValues = DepthMapHelpers.getDepthInKernel(depthFrame, (int)coordinate.x, (int)coordinate.y, nKernelSize: 5);
            // Averaging over the local neighbourhood improves stability 
            float averageDepth = DepthMapHelpers.averageValidDepthFromNeighbourhood(depthValues);
            ystem.Windows.Media.Media3D.Point3D worldCoordinates = 
        DepthMapHelpers.WorldCoordinate(averageDepth, (int)coordinate.x, (int)coordinate.y, intrinsicsDepthImagerMaster.fx, 
                                        intrinsicsDepthImagerMaster.fy, intrinsicsDepthImagerMaster.ppx, intrinsicsDepthImagerMaster.ppy);
        }
    }