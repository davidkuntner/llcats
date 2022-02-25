using H.Pipes;
using H.Pipes.Args;
using llcats.Common;

namespace llcats.WindowsService
{
    public class NamedPipesServer : IDisposable
    {
        const string PIPE_NAME = "samplepipe";
        private PipeServer<PipeMessage> pipeServer;

        public void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
        }

        public async Task DisposeAsync()
        {
            if(pipeServer != null)
            {
                await pipeServer.DisposeAsync();
            }
        }

        public async Task InitializeAsync()
        {
            pipeServer = new PipeServer<PipeMessage>(PIPE_NAME);

            pipeServer.ClientConnected += async (o, args) => await OnClientConnectedAsync(args);
            pipeServer.ClientDisconnected += (o, args) => OnClientDisconnected(args);
            pipeServer.MessageReceived += (sender, args) => OnMessageReceived(args.Message);
            pipeServer.ExceptionOccurred += (o, args) => OnExceptionOccurred(args.Exception);

            await pipeServer.StartAsync();
        }

        private async void OnClientConnected(ConnectionEventArgs<PipeMessage> args)
        {
            Console.WriteLine($"Client {args.Connection.PipeName} is now connected!");

            await args.Connection.WriteAsync(new PipeMessage
            {
                Action = ActionType.SendText,
                Text = "Hi from server"
            });
        }

        private void OnClientDisconnected(ConnectionEventArgs<PipeMessage> args)
        {
            Console.WriteLine($"Client {args.Connection.PipeName} disconnected");
        }

        private void OnMessageReceived(PipeMessage? message)
        {
            switch (message.Action)
            {
                case ActionType.SendText:
                    Console.WriteLine($"Text from client: {message.Text}");
                    break;

                case ActionType.ShowTrayIcon:
                    throw new NotImplementedException();

                case ActionType.HideTrayIcon:
                    throw new NotImplementedException();
              

                default:
                    Console.WriteLine($"Unknown Action Type: {message.Action}");
                    break;
            }
        }

        private void OnExceptionOccurred(Exception ex)
        {
            Console.WriteLine($"Exception occured in pipe: {ex}");
        }
    }
}