using Dapr.Client;
using Grpc.Core;
using Polly;

class Program
{
    const string stateStoreName = "statestore";

    static async Task Main(string[] args)
    {
        // Initialize Dapr client
        var daprClient = new DaprClientBuilder()
            .UseGrpcEndpoint("http://{server}:{port}")
            .Build();

        // Execute the hotel booking workflow
        await HotelBookingWorkflowAsync(daprClient);
    }

    static async Task HotelBookingWorkflowAsync(DaprClient daprClient)
    {
        // Define keys for different states
        string reservationKey = "reservation" + Guid.NewGuid().ToString();
        string checkInKey = "checkIn" + Guid.NewGuid().ToString();
        string roomServiceKey = "roomService" + Guid.NewGuid().ToString();
        string checkOutKey = "checkOut" + Guid.NewGuid().ToString();

        // Step 1: User books a room
        var reservationNumber = Guid.NewGuid().ToString();
        Console.WriteLine($"Step 1: User books a room - Reservation Number: {reservationNumber}");

        // Execute the SaveStateAsync operation with retry logic
        await RetryPolicy.ExecuteAsync(async () =>
        {
            await daprClient.SaveStateAsync(stateStoreName, reservationKey, reservationNumber);

            // Simulate a random failure during booking
            if (ShouldFail())
            {
                Console.WriteLine("Booking failed due to a random failure.");
                throw new InvalidOperationException("Booking failed.");
            }
        });

        // Step 2: User checks in
        string checkedInDetails = null;

        // Execute the GetStateAsync operation with retry logic
        await RetryPolicy.ExecuteAsync(async () =>
        {
            checkedInDetails = await daprClient.GetStateAsync<string>(stateStoreName, checkInKey);
            Console.WriteLine($"Step 2: User checks in - Details: {checkedInDetails}");

            // Simulate a random failure during check-in
            if (ShouldFail())
            {
                Console.WriteLine("Check-in failed due to a random failure.");
                throw new InvalidOperationException("Check-in failed.");
            }
        });

        // Step 3: User orders room service
        string roomServiceOrder = null;

        // Execute the SaveStateAsync operation with retry logic
        await RetryPolicy.ExecuteAsync(async () =>
        {
            roomServiceOrder = await daprClient.GetStateAsync<string>(stateStoreName, roomServiceKey);
            Console.WriteLine($"Step 3: User orders room service - Order: {roomServiceOrder}");

            // Simulate a random failure during room service order
            if (ShouldFail())
            {
                Console.WriteLine("Room service order failed due to a random failure.");
                throw new InvalidOperationException("Room service order failed.");
            }
        });

        // Step 4: User checks out
        string checkOutDetails = null;

        // Execute the GetStateAsync operation with retry logic
        await RetryPolicy.ExecuteAsync(async () =>
        {
            checkOutDetails = await daprClient.GetStateAsync<string>(stateStoreName, checkOutKey);
            Console.WriteLine($"Step 4: User checks out - Details: {checkOutDetails}");

            // Simulate a random failure during check-out
            if (ShouldFail())
            {
                Console.WriteLine("Check-out failed due to a random failure.");
                throw new InvalidOperationException("Check-out failed.");
            }
        });

        Console.WriteLine("Check-out successful. Hotel stay completed.");
    }

    // Simulate a random failure with a 20% chance
    static bool ShouldFail()
    {
        var random = new Random();
        return random.Next(0, 100) < 20;
    }

    // Updated RetryPolicy to handle RpcException and InvalidOperationException
    static readonly AsyncPolicy RetryPolicy = Policy
        .Handle<RpcException>(ex => ex.StatusCode == StatusCode.InvalidArgument)
        .Or<InvalidOperationException>()  // Handle InvalidOperationException
        .WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (exception, timeSpan, retryCount, context) =>
            {
                Console.WriteLine($"Retry {retryCount} after exception: {exception.Message}");
            });
}
