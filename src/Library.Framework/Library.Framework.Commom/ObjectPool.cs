using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Library.Framework.Commom
{
    public class ObjectPool<T> where T : class
    {
        private readonly Factory _factory;

        private readonly Element[] _items;

        private T _firstItem;

        internal delegate T Factory();

        private struct Element
        {
            internal T Value;
        }

        internal ObjectPool(Factory factory): this(factory, Environment.ProcessorCount * 2)
        { }
        
        internal ObjectPool(Factory factory, int size)
        {
            _factory = factory;
            _items = new Element[size - 1];
        }

        public T Allocate()
        {
            var inst = _firstItem;
            if (inst == null || inst != Interlocked.CompareExchange(ref _firstItem, null, inst))
                inst = AllocateSlow();


            #if DETECT_LEAKS
            var tracker = new LeakTracker();
            leakTrackers.Add(inst, tracker);

            #if TRACE_LEAKS
            var frame = CaptureStackTrace();
            tracker.Trace = frame;
            #endif
            #endif

            return inst;
        }

        private T CreateInstance() => _factory();

        private T AllocateSlow()
        {
            var items = _items;

            for (var i = 0; i < items.Length; i++)
            {
                var inst = items[i].Value;
                if (inst != null)
                {
                    if (inst == Interlocked.CompareExchange(ref items[i].Value, null, inst))
                        return inst;
                }
            }

            return CreateInstance();
        }

#if DETECT_LEAKS
        private static readonly ConditionalWeakTable<T, LeakTracker> leakTrackers = new ConditionalWeakTable<T, LeakTracker>();

        private class LeakTracker : IDisposable
        {
            private volatile bool disposed;

#if TRACE_LEAKS
            internal volatile object Trace = null;
#endif

            public void Dispose()
            {
                disposed = true;
                GC.SuppressFinalize(this);
            }

            private string GetTrace()
            {
#if TRACE_LEAKS
                    return Trace == null ? "" : Trace.ToString();
#else
                    return "Leak tracing information is disabled. Define TRACE_LEAKS on ObjectPool`1.cs to get more info \n";
#endif
            }

            ~LeakTracker()
            {
                if (!this.disposed && !Environment.HasShutdownStarted)
                {
                    var trace = GetTrace();

                    //Debug.WriteLine($"TRACEOBJECTPOOLLEAKS_BEGIN\nPool detected potential leaking of {typeof(T)}. \n Location of the leak: \n {GetTrace()} TRACEOBJECTPOOLLEAKS_END");
                }
            }
        }
#endif

        internal void ForgetTrackedObject(T old, T replacement = null)
        {
            #if DETECT_LEAKS
            LeakTracker tracker;
            if (leakTrackers.TryGetValue(old, out tracker))
            {
                tracker.Dispose();
                leakTrackers.Remove(old);
            }
            else
            {
                var trace = CaptureStackTrace();
                //Debug.WriteLine($"TRACEOBJECTPOOLLEAKS_BEGIN\nObject of type {typeof(T)} was freed, but was not from pool. \n Callstack: \n {trace} TRACEOBJECTPOOLLEAKS_END");
            }

            if (replacement != null)
            {
                tracker = new LeakTracker();
                leakTrackers.Add(replacement, tracker);
            }
        #endif
        }

        private static Lazy<Type> _stackTraceType = new Lazy<Type>(() => Type.GetType("System.Diagnostics.StackTrace"));

        private static object CaptureStackTrace()
        {
            return Activator.CreateInstance(_stackTraceType.Value);
        }
    }
}
