# Medialooks.XMedia.DevTools - Developer Utilities Library

## Overview
This library provides high-level utilities and helper classes for working with the Medialooks.XMedia API, simplifying common tasks in media processing workflows.

## Features

- **Frame Generation**: Create synthetic video and audio frames for testing
- **Array Wrappers**: Safe handling of frame, packet, and handler arrays
- **Font Rendering**: Built-in pixel font for text rendering in video frames
- **Resource Management**: Automatic cleanup of native resources
- **Utility Methods**: Common operations for media processing

## Components

### 1. Frame Generation Utilities
- `GenerateXmVideoFrame()` - Creates synthetic video frames with timestamp and frame number
- `GenerateXmAudioFrame()` - Generates synthetic audio frames with test tones
- `GenerateBGRAVideoFrameData()` - Produces raw BGRA video data
- `GenerateAudioFrameData()` - Creates raw audio samples

### 2. Array Wrappers
- `FramesArray` - Wrapper for `XmFramesArray` with IEnumerable support
- `PacketsArray` - Wrapper for `XmPacketsArray` with IEnumerable support
- `HandlersArray` - Wrapper for `XmHandlersArray` with IEnumerable support

### 3. Helper Classes
- `Frame` - High-level frame wrapper with properties and cleanup
- `FrameAudioData` - Structured audio frame data
- `FrameVideoData` - Structured video frame data
- `PixelFont` - Built-in font for text rendering

### 4. Handler Utilities
- `HandlersGet()` - Safe handler enumeration
- `HandlerFrameInputs/Outputs()` - Input/output frame access
- `HandlerPacketInputs/Outputs()` - Input/output packet access

## Usage Examples

### Generating Test Frames
```csharp
// Generate video frame
var videoFrame = Utils.GenerateXmVideoFrame(
    frameNum: 1, 
    capturedTimeMs: 40,
    width: 1280,
    height: 720
);

// Generate audio frame
var audioFrame = Utils.GenerateXmAudioFrame(
    audioPosInSamples: 0,
    numSamples: 1764
);
```

### Working with Arrays
```csharp
// Get handlers from container
using (var handlers = Handlers.HandlersGet(container))
{
    foreach (var handler in handlers)
    {
        // Process each handler
    }
}

// Access frame outputs
using (var outputs = Handlers.HandlerFrameOutputs(handler))
{
    foreach (var frame in outputs)
    {
        // Process each output frame
    }
}
```

### Using Frame Wrapper
```csharp
var nativeFrame = Utils.GenerateXmVideoFrame(1, 40);
using (var frame = new Frame(nativeFrame))
{
    Console.WriteLine($"Frame type: {frame.Type}");
    Console.WriteLine($"Stream UID: {frame.StreamInfo.stream_uid}");
    
    foreach (var program in frame.Programs)
    {
        Console.WriteLine($"Program: {program.program_num}");
    }
}
```

## Best Practices

1. **Resource Management**:
   - Always use `using` blocks with disposable objects
   - Release native resources explicitly when not using wrappers

2. **Frame Generation**:
   - Reuse frame generators for consistent testing
   - Adjust frame parameters to match your pipeline requirements

3. **Error Handling**:
   - Check for null returns from utility methods
   - Validate array lengths before access

## Integration

1. Add reference to `Medialooks.XMedia.DevTools.dll`
2. Add using directive:
   ```csharp
   using Medialooks.XMedia.DevTools;
   ```
3. Use utility methods alongside core XMedia API calls

## Advanced Features

### Custom Frame Generation
Override the default frame generation methods to create frames with:
- Custom test patterns
- Specific metadata
- Alternative formats

### Font Rendering
Use the built-in pixel font for:
- Adding timestamps to test frames
- Creating debug overlays
- Generating test cards

## License
This library is provided under the MIT License. The underlying Medialooks.XMedia API may have separate licensing terms.

## Dependencies
- Medialooks.XMedia API
- .NET 8+ runtime
