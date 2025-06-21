# InkSaver

InkSaver is a tool designed to help you save ink when printing documents. 
It converts images to grayscale to reduce the black ink usage.

This side project motivation came from the price of ink cartridges induced by printing (lots) of drawings for my kids 🧒

## How to use
1. Download the latest release from the [releases page](https://github.com/roddone/InkSaver/releases)
2. drag & drop an image file on the .exe

It will launch the system print window with the image converted to grayscale.

## Cli

you can use it in CLI mode by running the executable with the image file as an argument:
```bash
ink-saver.exe path/to/image.png
```

you can add the '-y' argument to skip the confirmation dialog and exit the program directly after printing : 
```bash
ink-saver.exe path/to/image.png -y
```

example:
original image: ![original](https://raw.githubusercontent.com/roddone/InkSaver/main/.github/fish.png)
printed image : ![printed](https://raw.githubusercontent.com/roddone/InkSaver/main/.github/fish.gray.png)