# XMedia Playlist Player Example

This example demonstrates how to use the Medialooks.XMedia API to create a playlist player with advanced media processing capabilities, including playlist management and runtime conversion settings.

## Overview

The `PlaylistPlayer` class provides functionality to:
1. Load and process JSON scheme files for media playback
2. Manage playlists of media items
3. Apply conversion settings during playback
4. Control playback position and settings programmatically
5. Handle complex media processing pipelines

## Prerequisites

- Medialooks.XMedia API installed
- .NET 8+ runtime
- JSON scheme file defining the processing pipeline
- Playlist file in JSON format
- Optional conversion settings file

## Usage

### Command Line Arguments
```
XMediaPlaylistPlayer.exe <scheme_file_path> <playlist_json> [<conversion_json>]
```

- `scheme_file_path`: Path to JSON file defining the processing scheme. See example in [playlist_player_to_ndi.json](./sample_schemes/playlist_player_to_ndi.json) file.
- `playlist_json`: Path to JSON playlist file. See example in [sample_playlist.json](./sample_schemes/sample_playlist.json) file.
- `conversion_json`: Optional path to JSON file with conversion settings. See example in [conversion_format.json](./sample_schemes/conversion_format.json ) file.

### Example
```
XMediaPlaylistPlayer.exe ./schemes/playlist_player_to_ndi.json ./schemes/sample_playlist.json ./schemes/conversion_settings.json
```

## Key Components

### JSON Scheme Processing
The example:
1. Reads the scheme template file
2. Injects the playlist content into the scheme
3. Creates a media handler from the complete scheme

### Playback Control
The example demonstrates:
1. Starting playback (`output_start`)
2. Applying conversion settings at runtime (`conversion_set`)
3. Retrieving current conversion settings (`conversion_get`)
4. Seeking to specific positions (`playback_start` with position)

### Playlist Management
The playlist JSON file should contain an array of media items with their properties. The scheme template must include a placeholder (`playlist_place_holder`) where the playlist content will be injected.

## Implementation Details

### Processing Pipeline
1. Loads and validates input files
2. Creates a container handler with the processing scheme
3. Executes control commands at specific timings
4. Monitors playback progress
5. Cleans up resources

### Timing Sequence
- At 2 seconds: Applies conversion settings (if provided)
- At 3 seconds: Retrieves current conversion settings
- At 5 seconds: Seeks to 2-second position
- Runs for 110 seconds total (demonstration purposes)

## Customization

To adapt this example:
1. Modify the timing sequence for different control points
2. Add additional commands for more complex control
3. Change the scheme template for different processing pipelines
4. Implement event callbacks for more reactive control

## Error Handling

The example includes comprehensive error handling:
- File reading validation
- Handler creation errors
- Command execution errors
- Resource cleanup
- General exception handling

## Best Practices

1. Always release handlers and resources
2. Validate JSON inputs before processing
3. Use proper error handling for file operations
4. Monitor command execution results
5. Consider implementing status callbacks for production use

## Output

The processor will:
1. Display the processed scheme (for debugging)
2. Show command execution results
3. Print progress messages
4. Report completion or errors

## Troubleshooting

- **Scheme errors**: Verify the scheme template and playlist format
- **Playback issues**: Check media file paths in the playlist
- **Command failures**: Examine the returned JSON error descriptions
- **Resource leaks**: Ensure all handlers are properly released

## License

This example is provided under the MIT License. The underlying Medialooks.XMedia API may have separate licensing terms.
