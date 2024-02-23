using Polly;
using Polly.Retry;
using Polly.Wrap;
using Polly.CircuitBreaker;

namespace poc_sync_spot_instance_retry_api.Resilience
{
    public static class ResilienceExtensions
    {
        public static AsyncPolicyWrap CreateResiliencePolicy(IEnumerable<TimeSpan> sleepsBeetweenRetries, int maxNumberExceptionsCircuitBreak, int secondsCircuitBreakerOpen)
        {
            AsyncRetryPolicy retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    sleepDurations: sleepsBeetweenRetries,
                    onRetry: (_, span, retryCount, _) =>
                    {
                        var previousBackgroundColor = Console.BackgroundColor;
                        var previousForegroundColor = Console.ForegroundColor;

                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;

                        Console.Out.WriteLineAsync($" ***** {DateTime.Now:HH:mm:ss} | " +
                            $"Retentativa: {retryCount} | " +
                            $"Tempo de Espera em segundos: {span.TotalSeconds} **** ");

                        Console.BackgroundColor = previousBackgroundColor;
                        Console.ForegroundColor = previousForegroundColor;
                    });

            AsyncCircuitBreakerPolicy circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(maxNumberExceptionsCircuitBreak, TimeSpan.FromSeconds(secondsCircuitBreakerOpen),
                    onBreak: (_, _) =>
                    {
                        ShowCircuitState("Aberto (onBreak)", ConsoleColor.Red);
                    },
                    onReset: () =>
                    {
                        ShowCircuitState("Fechado (onReset)", ConsoleColor.Green);
                    },
                    onHalfOpen: () =>
                    {
                        ShowCircuitState("Semi aberto (onHalfOpen)", ConsoleColor.Magenta);
                    });

            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }
        private static void ShowCircuitState(string descStatus, ConsoleColor backgroundColor)
        {
            var previousBackgroundColor = Console.BackgroundColor;
            var previousForegroundColor = Console.ForegroundColor;

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Out.WriteLine($" ***** Estado do Circuito: {descStatus} **** ");

            Console.BackgroundColor = previousBackgroundColor;
            Console.ForegroundColor = previousForegroundColor;
        }
    }
}