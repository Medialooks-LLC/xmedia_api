readme: |
  # Medialooks.XMedia C# API Wrapper

  This project provides a modern C# wrapper around the native [Medialooks XMedia](https://github.com/Medialooks-LLC/xmedia) C++ library. It enables powerful cross-platform media processing capabilities in .NET applications by exposing a managed interface for working with audio/video frames, packets, handlers, and callbacks.

<svg width="800" height="500" xmlns="http://www.w3.org/2000/svg">
  <style>
    .label {
      font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;
      font-size: 16px;
      fill: #333;
      text-anchor: middle;
    }
    .shadow {
      filter: drop-shadow(0 2px 4px rgba(0,0,0,0.2));
    }
  </style>
  <rect width="100%" height="100%" fill="#f9fafb"/>

<svg width="1000" height="500" xmlns="http://www.w3.org/2000/svg">
  <style>
    .label {
      font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;
      font-size: 16px;
      fill: #333;
      text-anchor: middle;
    }
    .shadow {
      filter: drop-shadow(0 2px 4px rgba(0,0,0,0.2));
    }
  </style>
  <rect width="100%" height="100%" fill="#f9fafb"/>

  <!-- Top Box -->
  <rect x="350" y="30" rx="16" ry="16" width="300" height="70" fill="#dbeafe" class="shadow"/>
  <text x="500" y="60" class="label" font-weight="bold">xMedia (C++ Library)</text>
  <text x="500" y="82" class="label">Core Implementation</text>

  <!-- Middle Box -->
  <rect x="350" y="130" rx="16" ry="16" width="300" height="70" fill="#dcfce7" class="shadow"/>
  <text x="500" y="160" class="label" font-weight="bold">xMedia API Wrappers</text>
  <text x="500" y="182" class="label">C# / Python / Other Bindings</text>

  <!-- Connectors -->
  <line x1="500" y1="100" x2="500" y2="130" stroke="#cbd5e1" stroke-width="2"/>
  <line x1="500" y1="200" x2="300" y2="260" stroke="#cbd5e1" stroke-width="2"/>
  <line x1="500" y1="200" x2="500" y2="260" stroke="#cbd5e1" stroke-width="2"/>
  <line x1="500" y1="200" x2="700" y2="260" stroke="#cbd5e1" stroke-width="2"/>

  <!-- Bottom Left -->
  <rect x="200" y="260" rx="16" ry="16" width="200" height="70" fill="#f1f5f9" class="shadow"/>
  <text x="300" y="300" class="label">C# Applications</text>

  <!-- Bottom Center -->
  <rect x="400" y="260" rx="16" ry="16" width="200" height="70" fill="#f1f5f9" class="shadow"/>
  <text x="500" y="300" class="label">Python Applications</text>

  <!-- Bottom Right -->
  <rect x="600" y="260" rx="16" ry="16" width="200" height="70" fill="#f1f5f9" class="shadow"/>
  <text x="700" y="300" class="label">Other Application</text>
</svg>


  ## About XMedia

  **xMedia** is a professional, multi-platform video SDK designed for developers who require high-performance media workflows on Windows, macOS, and Linux.

  To learn more about the SDK and its roadmap, visit:
  [https://medialooks.com/xmedia-sdk](https://medialooks.com/xmedia-sdk)
  [xMedia C++ API] [https://medialooks-llc.github.io/xmedia/]

  ## Features

  - Cross-platform support (Windows, macOS, Linux)
  - Video and audio frame processing
  - Handler system for building media pipelines
  - Flexible callback registration for frames, packets, timers, and events
  - Frame serialization, UID generation, and error handling utilities
  - Reference-counted memory management and safe cleanup

  ## Prerequisites

  - [.NET 8](https://dotnet.microsoft.com/download) or later
  - Native `xmedia_c_api` library for your platform (`.dll`, `.dylib`, or `.so`)
  - Basic familiarity with media processing concepts such as frames, packets, codecs, and handlers

  ## Installation

  1. Add the `Medialooks.XMedia` project to your solution.
  2. Reference the wrapper project in your .NET application.
  3. Ensure the appropriate `xmedia_c_api` native library is available in your build output or runtime search path.

  ## Basic Usage

  Below are examples demonstrating how to create frames, handlers, and register callbacks.

  ### Creating a Video Frame

  ```csharp
  var error = new Error();

  var uids = new Uids
  {
      stream_uid = 1001,
      group_uid = 1001,
      source_uid = 1001
  };

  var time = new Time
  {
      timestamp = 1000
  };

  var format = new FormatV
  {
      width = 1920,
      height = 1080,
      pixel_format = "yuv420p"
  };

  var videoData = new XmVideoData
  {
      format = format
  };

  var frame = FrameFunctions.XmFrameVideoAlloc(
      uids,
      time,
      videoData,
      null,
      null,
      null,
      ref error
  );

  if (error.Code != 0)
  {
      Console.WriteLine($"Error: {error}");
  }
  ```

  ### Creating a Handler

  ```csharp
  var handlerDesc = new HandlerDesc
  {
      handler_type = "av_demultiplexer",
      handler_name = "input",
      open_url_or_json_scheme = "file.mp4"
  };

  var handler = Handlers.XmHandlerCreateByDesc(handlerDesc, ref error);

  if (error.Code != 0)
  {
      Console.WriteLine($"Handler creation failed: {error}");
  }
  ```

  ### Implementing a Frame Callback

  ```csharp
  object userState = new MyCallbackState();

  var callback = Callbacks.FrameCallbackAdd(
      handler,
      ObjectType.FrameVideo,
      "",
      ref userState,
      (ref object state, XmFrame frame) =>
      {
          Console.WriteLine($"Received frame at timestamp {frame.time.timestamp}");
          return true;
      },
      ref error
  );

  if (error.Code != 0)
  {
      Console.WriteLine($"Callback registration failed: {error}");
  }
  ```

  ## Key Components

  ### Frame and Packet Management

  **FrameFunctions**
  Create, copy, release, and manage video and audio frames.

  **PacketFunctions**
  Allocate and manipulate media packets.

  **Functions**
  Utility helpers for general media operations.

  ### Handler System

  **Handlers**
  Create, manage, and execute commands on media handlers. Handlers are the main way to build processing pipelines, such as demultiplexing, decoding, or encoding.

  **Handler Descriptions**
  Provide configuration data for instantiating handlers.

  ### Callback System

  - Frame callbacks: Receive and process video or audio frames.
  - Packet callbacks: Work with raw media packets.
  - Timer callbacks: Schedule time-based operations.
  - Event callbacks: Respond to asynchronous media events.

  ### Utilities

  - Utilities for memory management and error handling.
  - Interop structures for format descriptions and media properties.
  - Enumerations defining color spaces, pixel formats, and codec parameters.

  ## Error Handling

  All operations return an `Error` object that includes:

  - **Code**: Numeric error code (`0` indicates success)
  - **Category**: Description of the error category

  Always check the error code:

  ```csharp
  if (error.Code != 0)
  {
      Console.WriteLine($"Operation failed: {error}");
  }
  ```

  ## Memory Management

  Resources use reference counting. Always release objects after use:

  ```csharp
  FrameFunctions.XmFrameReleaseSafe(ref frame);
  Handlers.XmHandlerReleaseSafe(ref handler);
  ```

  ## Example Projects

  - Medialooks.XMedia.DevTools (developer utilities)
  - XMedia Frame Processing Example
  - XMedia Playlist Player Example
  - XMedia Timer-Based Frame Processor Example
  - XMedia Event Processor Example

  ## Building

  You can build this project in Visual Studio 2022 or newer. Alternatively, use the .NET CLI:

  ```
  dotnet build
  ```

  ## License

  This wrapper is distributed under the MIT License.
  The underlying native `xmedia_c_api` library is subject to its own licensing terms.

  ## Additional Information

  This C# wrapper is designed for developers who want to integrate the high-performance capabilities of the [xMedia C++ SDK](https://github.com/Medialooks-LLC/xmedia) into .NET applications. Whether you are building professional broadcast systems, video servers, or custom playback solutions, this library provides a modern, managed interface to advanced media processing.

  To learn more about the SDK and its roadmap, visit:
  [https://medialooks.com/xmedia-sdk](https://medialooks.com/xmedia-sdk)

  ## Support

  If you encounter issues or have questions, please contact us at [support@medialooks.com](mailto:support@medialooks.com)
