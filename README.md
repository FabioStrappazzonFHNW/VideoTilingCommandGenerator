# Video Tiling Command Generator

This program was created as part of the bachelor's thesis by Sandro Schwager and Fabio Strappazzon.

## Usage

This tool generates an FFmpeg command that in turn will generate videos from a sequence of JPEG 2000 images. FFmpeg will split the videos into tiles of a given resolution.


**usage:** VideoTilingCommandGenerator tileResolution [tilings...]  
**tileResolution:** Resolution in Pixel of each tile  
**tilings:** list of tilings (ex "1 2" to generate 5 tiles, one containig the whole picture and four containig a quarter of the picture each.)  
**example:** VideoTilingCommandGenerator 512 1 2 4 8 (generates a command that will create 85 tiles with 512x512 resolution each)

## Dependencies
To use the generated command you will need FFmpeg installed.

### Notes
On Windows the generated command will likely be too large for your command line to handle. Install Bash or any similar Shell to execute the command. For extremely large commands either switch to UNIX or rewrite this script to generate a series of shorter commands that each only create a portion of the tiles.