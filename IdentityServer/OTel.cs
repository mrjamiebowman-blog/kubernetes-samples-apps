using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace MrJB.Kubernetes.IdentityServer;

internal sealed class OTel
{
    /// <summary>
    ///  The assembly name.
    /// </summary>
    internal static readonly AssemblyName AssemblyName = typeof(OTel).Assembly.GetName();

    /// <summary>
    ///  The activity source name.
    /// </summary>
    internal static readonly string ActivitySourceName = AssemblyName?.Name ?? "MrJB.Kubernetes.IdentityServer";

    /// <summary>
    ///  The assembly version.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal static string GetVersion<T>()
    {
        return typeof(T).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion.Split('+')[0];
    }

    /// <summary>
    /// The activity source.
    /// </summary>
    internal static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName, GetVersion<OTel>());

    internal static class MetricNames
    {
        internal const string BaseName = "mrjb.k8s.identityserver";
    }

    internal static class Meters
    {
        internal static Meter Consumer = new Meter(ActivitySourceName, GetVersion<OTel>());

        //private static Counter<int> GetOrders = Application.CreateCounter<int>(OTel.MetricNames.GetOrders, description: "Tracks when a order is retrieved.");

        //private static Counter<int> SaveOrder = Application.CreateCounter<int>(OTel.MetricNames.SaveOrder, description: "Tracks when a order is saved.");

        //internal static void AddGetOrders(int count = 1) => GetOrders.Add(count);
        //internal static void AddGetOrders(int count = 1, TagList tagList = new TagList()) => GetOrders.Add(count, tagList);

        //internal static void AddSaveOrders(int count = 1) => SaveOrder.Add(count);
        //internal static void AddSaveOrders(int count = 1, TagList tagList = new TagList()) => SaveOrder.Add(count, tagList);
    }
}
