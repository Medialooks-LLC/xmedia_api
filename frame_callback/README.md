# XMedia Frame Processing Example

This example demonstrates how to use the Medialooks.XMedia API to process video frames from a media file, save snapshots at regular intervals, and manage media handlers and callbacks.

## Overview

The `FrameProcessor` class provides functionality to:
1. Open a media file using a container handler
2. Set up a decoder for the media stream
3. Process video frames through a callback
4. Save every 30th frame as a PNG image
5. Manage resources and error handling

## Prerequisites

- Medialooks.XMedia API installed
- .NET 8+ runtime
- Valid media file for processing

## Usage

### Command Line Arguments
```
XMediaFrameProcessor.exe <file_path> <output_directory> [timeout_seconds]
```

- `file_path`: Path to the input media file
- `output_directory`: Directory where captured frames will be saved
- `timeout_seconds`: Optional timeout in seconds (default: 5)

### Example
```
XMediaFrameProcessor.exe "C:\videos\input.mp4" "C:\captures\" 10
```

## Key Components

### FrameProcessingStats
Tracks processing state:
- `ContinueFrameReceiving`: Controls when to stop processing
- `FrameCount`: Total frames processed
- `VideoFramesReceived`: Video-specific frames processed
- `OutputDir`: Where to save captured frames

### Frame Callback
The `FrameCallbackHandler`:
1. Checks frame type (video/audio)
2. Saves every 30th video frame as PNG
3. Updates processing statistics
4. Releases frame resources
5. Handles errors gracefully

### Processing Pipeline
1. Creates a container handler with demuxer and decoder
2. Sets up frame callback on the decoder
3. Starts processing with `output_start` command
4. Monitors progress with timeout
5. Cleans up resources

## Error Handling

The example includes comprehensive error handling:
- Input validation
- Handler creation errors
- Callback setup errors
- Frame processing exceptions
- Resource cleanup

## Customization

To adapt this example:
1. Change the frame capture interval (currently every 30 frames)
2. Modify the output format or quality
3. Add additional processing to the callback
4. Adjust the container scheme for different input types

## Output

The processor will:
1. Display progress messages in the console
2. Save captured frames as `captured_XXXX.png` in the output directory
3. Report success or failure upon completion

## Best Practices

1. Always release handlers and frames
2. Use timeouts for long operations
3. Validate input paths
4. Handle exceptions gracefully
5. Monitor resource usage

## Troubleshooting

- **No frames processed**: Check input file format and path
- **Callback errors**: Verify frame types and processing logic
- **Resource leaks**: Ensure all handlers and frames are released
- **Permission issues**: Confirm write access to output directory

## License

This example is provided under the MIT License. The underlying Medialooks.XMedia API may have separate licensing terms.
