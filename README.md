[![.NET](https://github.com/movsal08/DaprPilot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/movsal08/DaprPilot/actions/workflows/dotnet.yml)

# Dapr Pilot Hotel Booking App

## Overview

The Dapr Pilot Hotel Booking App demonstrates the power of Dapr (Distributed Application Runtime) in orchestrating a seamless hotel booking and stay experience. The application utilizes Dapr's features for state management, retry logic, and distributed coordination to handle various stages of a hotel stay.

### Workflow Steps:

1. **User Room Reservation**: Initiates the process by booking a room with a unique reservation number.
2. **User Check-In**: Simulates the user checking into the booked room with retry logic to handle failures.
3. **Room Service Order**: Allows the user to place a room service order, showcasing Dapr's state management.
4. **User Check-Out**: Concludes the hotel stay, managing failures during the check-out process.

## How It Works

The application follows a workflow-driven approach using Dapr. Key components include:

- **Dapr Client Initialization**: Establishes communication with the Dapr runtime using gRPC.
- **State Store Operations**: Leverages Dapr's state store for reliable data persistence.
- **Retry Logic**: Implements retry policies to gracefully handle transient failures.
- **Workflow Execution**: Demonstrates how Dapr coordinates booking, check-in, room service, and check-out.

## Getting Started

To run the Dapr Pilot Hotel Booking App on your local machine, follow these steps:

1. **Clone this repository:**
    ```bash
    git clone [repository_url]
    cd [repository_directory]
    ```

2. **Install Dapr CLI:**
   Follow the official [Dapr installation guide](https://docs.dapr.io/getting-started/install-dapr-cli/) to install the Dapr CLI.

3. **Update Dapr Server URL:**
   Open the `Program.cs` file find the following code and update URL and port:
   ```csharp
   // Initialize Dapr client
   var daprClient = new DaprClientBuilder()
       .UseGrpcEndpoint("http://{server}:{port}")
       .Build();

4. **Run the Dapr Pilot Hotel Booking App:**
   Navigate to the `DaprPilot` directory and execute the `Program.cs` file.

### Prerequisites

Ensure you have the following installed:

- Dapr Runtime
- Docker
- Docker Compose

### Dependencies

- Dapr: An open-source, event-driven runtime for building distributed applications.
- .NET Core: The cross-platform framework used for building and running the C# code.

## Contributions

Contributions, bug reports, and feature requests are welcomed! Feel free to open issues and submit pull requests.

## License

This project is licensed under the MIT License.

## Acknowledgments

This Dapr Pilot Hotel Booking App aims to provide an educational example of how Dapr can be employed for orchestrating distributed and fault-tolerant business processes.
