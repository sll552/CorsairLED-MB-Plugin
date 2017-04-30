# Corsair LED MusicBee Plugin
This Repo contains the CorsairLED Musicbee Plugin. The Plugin aims to provide support for spectrogram visualization on Corsair RGB LED Devices using the CUE SDK.

## Supported Devices
All Corsair CUE enabled Keyboards, tested only with K95 Platinum

## Installation
### Prerequisites
* Running Corsair Utility Engine
   * with enabled SDK (Global Settings -> enable SDK)
* Keyboard supported by CUE
* MusicBee 3

### Installation
Download the latest release .zip and extract it into your Plugin folder (usually C:\Program Files (x86)\MusicBee\Plugins). It should get loaded by MusicBee automatically.

## Plugin Settings
![Settings](https://i.imgur.com/OUldnr6.png)
* **Primary Color:** The color used for the effect/animated LEDs 
* **Background Color:** The color used for all other Keys
* **Coloring Mode:** Coloring effect for the animated LEDs currently:
  * *Solid*: Use the primary color for the LEDs.
  * *Gradient*: Use a static gradient starting with the primary color.
  * *Random (beta)*: Random generated color, does not look good currently.
* **K95P Lightbar as track progress:** Use the Lightbar of the K95 Platinum to indicate the progress in the currently playing track

## 3rd Party Libraries used #
* [CUE.NET](https://github.com/DarthAffe/CUE.NET)

## Contributing
Feel free to send me pull requests or open issues. 
This repo is a Visual Studio 2017 Solution.

## License
Apache 2.0
