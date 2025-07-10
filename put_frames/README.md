# XMedia Timer-Based Frame Processor Example

This example demonstrates how to use the Medialooks.XMedia API to create a precise timer-based media processing pipeline that generates and processes video/audio frames at exact intervals.

## Overview

The `TimerProcessor` class provides functionality to:
1. Generate synthetic video and audio frames at precise intervals
2. Process frames through a custom media pipeline defined in a JSON scheme
3. Maintain perfect timing synchronization between frames
4. Handle both video and audio streams simultaneously
5. Monitor processing progress and handle errors

## Prerequisites

- Medialooks.XMedia API installed
- .NET 6+ runtime
- JSON scheme file defining the processing pipeline
- DevTools utilities for frame generation

## Usage

### Command Line Arguments
```
XMediaTimerProcessor.exe <scheme_file_path> [timeout_seconds]
```

- `scheme_file_path`: Path to JSON file defining the processing scheme. See example in [xm_encode_to_file.json](./sample_schemes/xm_encode_to_file.json) file.
- `timeout_seconds`: Optional timeout in seconds (default: 5)

### Example
```
XMediaTimerProcessor.exe ./schemes/xm_encode_to_file.json 10
```

## Key Components

### Timer Callback System
- Uses `OnTimerCallback` to maintain precise frame timing
- Generates frames at exact 40ms intervals (25fps)
- Handles both video and audio frame generation
- Automatically stops after 300 frames (12 seconds)

### Frame Generation
- `GenerateXmVideoFrame()`: Creates synthetic video frames with frame number and timestamp
- `GenerateXmAudioFrame()`: Creates synthetic audio frames synchronized with video

### Processing Pipeline
1. Loads and validates the processing scheme
2. Creates a frames handler with initial frames
3. Starts the processing pipeline
4. Sets up the precision timer callback
5. Monitors processing progress
6. Cleans up resources

## Implementation Details

### Timing Control
- Uses system clock for precise timing (microsecond precision)
- Calculates exact frame presentation times
- Maintains consistent frame duration (40ms = 25fps)

### Error Handling
- Validates frame submission to handler
- Tracks processing errors in callback data
- Provides detailed error messages
- Ensures proper resource cleanup

### Progress Monitoring
- Displays real-time processing statistics
- Shows frames processed per second
- Tracks total processing time

## Customization

To adapt this example:
1. Change the frame rate by modifying `FrameDuration`
2. Adjust the total frame count by changing the callback limit
3. Modify the frame generation functions for different content
4. Add additional processing steps in the callback
5. Implement more sophisticated error recovery

## Best Practices

1. Use high-precision timers for media processing
2. Validate all frame submissions
3. Monitor resource usage
4. Implement proper cleanup in all cases
5. Provide detailed progress feedback

## Output

The processor will:
1. Display the processing scheme being used
2. Show real-time progress updates
3. Report any errors during processing
4. Confirm completion or timeout

## Troubleshooting

- **Timing issues**: Verify system clock precision
- **Frame submission errors**: Check scheme compatibility
- **Resource problems**: Monitor handler creation
- **Performance issues**: Adjust frame rate or complexity

## License

This example is provided under the MIT License. The underlying Medialooks.XMedia API may have separate licensing terms.

## Advanced Features

- Precise frame synchronization (video+audio)
- Microsecond-accurate timing
- Dynamic frame generation
- Runtime pipeline control
- Comprehensive error tracking
