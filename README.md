# Corsair LED MusicBee Plugin
This Repo contains the CorsairLED Musicbee Plugin. The Plugin aims to provide support for spectrogram visualization on Corsair RGB LED devices using the CUE SDK.

## Supported Devices
All Corsair CUE enabled devices, tested only with K95 Platinum and CUE Demo devices

## Installation
### Prerequisites
* Running Corsair Utility Engine
   * with enabled SDK (Global Settings -> enable SDK)
* Device supported by CUE
* MusicBee 3

### Installation
Download the latest release .zip and extract it into your Plugin folder (usually C:\Program Files (x86)\MusicBee\Plugins). It should get loaded by MusicBee automatically.

### Update from 0.1.x to 1.0.0
Version 1.0.0 uses a new settings file format. It should automatically convert the old one but if this fails you will loose your settings and have to set them again and save.

## Plugin Settings
![Settings](https://i.imgur.com/OcVRMJN.png)
* **Primary Color:** The color used for the effect/animated LEDs 
* **Background Color:** The color used for all other Keys
* **Coloring Mode:** Coloring effect for the animated LEDs currently:
  * *Solid*: Use the primary color for the LEDs.
  * *Gradient*: Use a static gradient starting with the primary color.
  * *Random (beta)*: Random generated color, does not look good currently.
* **K95P Lightbar as track progress:** Use the Lightbar of the K95 Platinum to indicate the progress in the currently playing track.
* **Default:** If a default device is set all other devices that have no explicit set color options will inherit them from this device.
* **Effect:** Select the effect to be shown on this device. Selecting "None" will light the device in the selected background color but won't show any animation.
* **Enabled:** If you disable a device no ligthing will be performed on this device.

## 3rd Party Libraries used #
* [CUE.NET](https://github.com/DarthAffe/CUE.NET)

## Contributing
Feel free to send me pull requests or open issues. 
This repo is a Visual Studio 2017 Solution.

## License
Apache 2.0
