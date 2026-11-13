using Minio.DataModel.Args;
using System.Reflection;

namespace DataUploader.FileStorage.Tests.Helpers
{
    public class CallbackHelpers
    {
        public static Action<Stream> ExtractCallback(GetObjectArgs args)
        {
            var fields = typeof(GetObjectArgs)
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            var funcField = fields.FirstOrDefault(f => f.FieldType == typeof(Func<Stream, Task>));
            if (funcField?.GetValue(args) is Func<Stream, Task> func)
                return stream => func(stream).GetAwaiter().GetResult();

            var funcCtField = fields.FirstOrDefault(
                f => f.FieldType == typeof(Func<Stream, CancellationToken, Task>));
            if (funcCtField?.GetValue(args) is Func<Stream, CancellationToken, Task> funcCt)
                return stream => funcCt(stream, CancellationToken.None).GetAwaiter().GetResult();

            var actionField = fields.FirstOrDefault(f => f.FieldType == typeof(Action<Stream>));
            if (actionField?.GetValue(args) is Action<Stream> action)
                return action;

            var dump = string.Join(Environment.NewLine, fields.Select(f => $"{f.Name} : {f.FieldType}"));
            throw new InvalidOperationException(
                "Не удалось извлечь callback из GetObjectArgs. Поля класса:" +
                Environment.NewLine + dump);
        }
    }
}
