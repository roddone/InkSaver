// See https://aka.ms/new-console-template for more information
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;

bool noPrompt = args.Any(a => a == "-y");

try
{
    string document = args.Length >= 1 ? args[0] : string.Empty;

    if (string.IsNullOrEmpty(document) || !File.Exists(document))
    {
        Console.WriteLine($"Document '{document}' not found");

        PromptExitConfirm();

        return (int)ExitCode.InvalidDocument;
    }

    string tempFilePath = GetTempFilePath(document);

    using (Image<Rgba32> image = Image.Load<Rgba32>(document))
    {
        image.Mutate(x => x
            // convert to gray scale
            .Grayscale(0.5f)


            // reduce contrast
            .Contrast(0.25f)

            // increase brightness
            .Brightness(2f)
        );

        await image.SaveAsync(tempFilePath, new PngEncoder
        {
            ColorType = PngColorType.Grayscale, // Pour forcer une sortie en PNG "gris"
            CompressionLevel = PngCompressionLevel.DefaultCompression
        });
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
