//https://batiukvision.cognitiveservices.azure.com/vision/v3.2/analyze?details=Celebrities

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Linq;

string endpoint = "https://batiukvision.cognitiveservices.azure.com/";
string key = "key";
string descriptionUrl = "https://armyinform.com.ua/wp-content/uploads/2023/05/74946b04-0c30-4cd9-81de-cd72de5b29d6-e1684652289439-400x300.jpg";

ImageAnalysis analysis = await GetImageAnalisisAsync(key, endpoint, descriptionUrl);
GetImageDescriptionAsync(analysis);
GetFacesInfo(analysis);
GetColorSchemeAsync(analysis);
GetCelebritiesAsync(analysis);
GetLandMarksAsync(analysis);

Console.ReadLine();
ComputerVisionClient CreateVisionClient(string key, string endpoint)
{
    ApiKeyServiceClientCredentials clientCredentials = new ApiKeyServiceClientCredentials(key);
    ComputerVisionClient visionClient = new ComputerVisionClient(clientCredentials)
    {
        Endpoint = endpoint
    };
    return visionClient;
}

async Task<ImageAnalysis> GetImageAnalisisAsync(string key, string endpoint,string url)
{
    ComputerVisionClient visionClient = CreateVisionClient(key,endpoint);
    IList<VisualFeatureTypes?> featureTypes = Enum.GetValues(typeof(VisualFeatureTypes)).OfType<VisualFeatureTypes?>().ToList();
    ImageAnalysis analysis = await visionClient.AnalyzeImageAsync(url, featureTypes);
    return analysis;
}

void GetImageDescriptionAsync(ImageAnalysis analysis)
{
    Console.WriteLine("Image Description");
    foreach(var details in analysis.Description!.Captions)
    {
        Console.WriteLine($"{details.Text} with confmidece: {details.Confidence}");
    }
}

void GetFacesInfo(ImageAnalysis analysis)
{
    Console.WriteLine("Recognizes faces");
    foreach (var face in analysis.Faces)
    {
        Console.WriteLine($"Gender: {face.Gender} Age: {face.Age} Top: {face.FaceRectangle.Top}, Left : {face.FaceRectangle.Left}");
    }
}

void GetColorSchemeAsync(ImageAnalysis analysis)
{
    Console.WriteLine("\n=> Colors:");
    Console.WriteLine($"Accent color: {analysis.Color.AccentColor}");
    Console.WriteLine($"Prevailing background colour:" +
    $" {analysis.Color.DominantColorBackground}");
    Console.WriteLine($"Prevailing primary colour: " +
    $"{analysis.Color.DominantColorForeground}");
    Console.WriteLine("Primary Colors: " +
    string.Join(", ", analysis.Color.DominantColors));
    Console.WriteLine("The image is " +
    (analysis.Color.IsBWImg ? "black-white" : "colorful"));
}
void GetCelebritiesAsync(ImageAnalysis analysis)
{
    Console.WriteLine("\n=> Celebrities:");
    foreach (Category category in analysis.Categories)
    {
        if (category.Detail?.Celebrities != null)
        {
            foreach (var celeb in category.Detail.Celebrities)
            {
                Console.WriteLine($"Celebrity {celeb.Name} " +
                $"with confidence {celeb.Confidence} is in region " +
                $"({celeb.FaceRectangle.Left}, {celeb.FaceRectangle.Top})" + $"({celeb.FaceRectangle.Left + celeb.FaceRectangle.Width}, " + $"{celeb.FaceRectangle.Top + celeb.FaceRectangle.Height})");
            }
        }
    }
}
void GetLandMarksAsync(ImageAnalysis analysis)
{
    Console.WriteLine("\n=> LandMarks:");
    foreach (var category in analysis.Categories)
    {
        if (category.Detail?.Landmarks != null)
        {
            foreach (var landMark in category.Detail.Landmarks)
            {
                Console.WriteLine($"LandMark {landMark.Name} " +
                $"with confidence {landMark.Confidence}");
            }
        }
    }
}