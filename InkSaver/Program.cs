// See https://aka.ms/new-console-template for more information
using ImageMagick;
using System.Diagnostics;

bool noPrompt = args.Any(a => a == "-y");

try
{
    string document = args.Length >= 1 ? args[0] : string.Empty; // @"C:\Users\Romain\Downloads\tests-images\reine des neiges.webp";

    if (string.IsNullOrEmpty(document) || !File.Exists(document))
    {
        Console.WriteLine($"Document '{document}' not found");

        PromptExitConfirm();

        return (int)ExitCode.InvalidDocument;
    }

    string tempFilePath = GetTempFilePath(document);
    using (var imageM = new MagickImage(document))
    {
        //turn into grayScale
        imageM.Grayscale(PixelIntensityMethod.Lightness);
        //update brightness
        imageM.BrightnessContrast(new Percentage(50), new Percentage(-50));
        //write into png (some formats cannot be print, eg. webp)
        await imageM.WriteAsync(tempFilePath, MagickFormat.Png);
    }

    //launch print job
    Process printjob = new();
    printjob.StartInfo.FileName = tempFilePath;
    printjob.StartInfo.UseShellExecute = true;
    printjob.StartInfo.Verb = "print";
    printjob.StartInfo.CreateNoWindow = true;
    printjob.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

    printjob.Start();

    PromptExitConfirm();

    //delete temp file after processing
    File.Delete(tempFilePath);

    return (int)ExitCode.Success;

}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
    PromptExitConfirm();

    return (int)ExitCode.Error;
}

void PromptExitConfirm(string msg = "Press Enter to quit.")
{
    if (!noPrompt)
    {
        Console.WriteLine(msg);
        Console.ReadLine();
    }

}
static string GetTempFilePath(string file)
    => Path.Combine(Path.GetDirectoryName(file) ?? "", Path.GetFileNameWithoutExtension(file) + ".temp.png");


public enum ExitCode : int
{
    Success = 0,
    Error = 1,
    InvalidDocument = 2,
    InvalidArgs = 3
}
