# XMedia Event Processor Example

## Overview
This example demonstrates how to use the Medialooks.XMedia API to handle media processing events, specifically focusing on EOF (End-of-File) event detection during media processing workflows.

## Features
- Event-driven media processing architecture
- EOF (End-of-File) event detection
- JSON scheme-based pipeline configuration
- Proper resource cleanup and error handling
- Real-time processing monitoring

## Prerequisites
- Medialooks.XMedia API installed
- .NET 8+ runtime
- JSON scheme file defining the processing pipeline

## Usage

### Command Line Arguments
```
XMediaEventProcessor.exe <scheme_file_path>
```

- `scheme_file_path`: Path to JSON file defining the processing scheme. See example in [transcoding_demo.json](./sample_schemes/transcoding_demo.json) file.

### Example
```
XMediaEventProcessor.exe ./scheme/transcoding_demo.json
```

## Key Components

### Event Callback System
- Registers callback for EOF events
- Uses `OnEventCallback` delegate for event handling
- Properly releases event resources after processing
- Handles errors in event callback

### Processing Pipeline
1. Loads and validates the processing scheme
2. Creates a media handler from the scheme
3. Sets up event callback for EOF detection
4. Starts the processing pipeline
5. Monitors for event completion
6. Cleans up resources

## Implementation Details

### Event Processing
- Tracks event reception with `EventProcessingStats`
- Specifically listens for `EOFReached` events
- Releases event objects immediately after processing
- Provides detailed error handling

### Resource Management
- Proper handler initialization and cleanup
- Safe event callback registration/deregistration
- Comprehensive error checking
- Using blocks for handler enumeration

## Customization Options

1. **Event Types**: Modify to listen for different event types
2. **Multiple Handlers**: Extend to handle events from multiple handlers
3. **Additional Processing**: Add custom logic in event callback
4. **Timeout**: Implement processing timeout
5. **Status Monitoring**: Add progress tracking

## Best Practices

1. Always release event objects after processing
2. Validate handler creation and commands
3. Implement comprehensive error handling
4. Use proper resource cleanup patterns
5. Monitor processing progress

## Output

The processor will:
1. Display the processing scheme being used
2. Show real-time waiting status
3. Confirm successful event reception
4. Report any errors during processing

## Troubleshooting

- **Event not received**: Verify scheme produces expected events
- **Handler issues**: Check scheme validity and handler creation
- **Resource leaks**: Ensure proper cleanup in all code paths
- **Callback errors**: Examine error messages in detail

## License

This example is provided under the MIT License. The underlying Medialooks.XMedia API may have separate licensing terms.

## Advanced Features

- Multiple event type handling
- Event data processing
- Dynamic scheme modification
- Pipeline status monitoring
- Complex error recovery
