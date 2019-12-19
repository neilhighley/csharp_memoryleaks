using NUnit.Framework;
// https://michaelscodingspot.com/find-fix-and-avoid-memory-leaks-in-c-net-8-best-practices/
// https://docs.microsoft.com/en-us/dotnet/standard/events/
// https://codewithshadman.com/memory-leak-c/
// https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals
// https://www.codingame.com/playgrounds/6179/garbage-collection-and-c
// https://michaelscodingspot.com/ways-to-cause-memory-leaks-in-dotnet
// https://www.jetbrains.com/dotmemory/unit/

// https://csharpindepth.com/articles/Events

// https://www.dotnetperls.com/weakreference
// https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/weak-references
// https://docs.microsoft.com/en-us/dotnet/api/system.weakreference?view=netframework-4.8


namespace memoryleaks
{
using System;
public interface ITestGCObject{
    void RunMethod();
}
    public class LeakyObject : ITestGCObject
    {
        public void RunMethod()
        {
            Console.WriteLine("Leaky Run");
        }
        public LeakyObject(){
            Console.WriteLine("LeakyObject Object Created");
        }
    }
    public class LeakyObjectWithEvent:ITestGCObject{
        public event EventHandler ConnectedToSource;
         protected virtual void OnConnectedToSource(EventArgs e)
        {
            EventHandler handler = ConnectedToSource;
            handler?.Invoke(this, e);
        }
        public void RunMethod()
        {
            Console.WriteLine("LeakyWithObject Run");
        }
        public LeakyObjectWithEvent(){
            Console.WriteLine("LeakyObjectWithEvent Object Created");
        }
    }
    public class NonLeakyObject:ITestGCObject{
        public NonLeakyObject(){
            Console.WriteLine("NonLeaky Object Created;");
        }
         public void RunMethod()
        {
            Console.WriteLine("NonLeaky Run;");
        }
    }

    public class NonLeakyObjectWithEvent:ITestGCObject{
        public event EventHandler ConnectedToSource;
         protected virtual void OnConnectedToSource(EventArgs e)
        {
            EventHandler handler = ConnectedToSource;
            handler?.Invoke(this, e);
        }
        public NonLeakyObjectWithEvent(){
            Console.Write("Leaky Object Created;");
        }
         public void RunMethod()
        {
            Console.Write("NonLeakyWithObject Run;");
        }
    }
    public class Tests
    {
        public LeakyObjectWithEvent leakyObjectWithEvent=new LeakyObjectWithEvent();
        public NonLeakyObjectWithEvent nonLeakyObjectWithEvent=new NonLeakyObjectWithEvent();
        
        [Test]
        public void NewLeaky_Nulled_IsNotGarbageCollected()
        {
            LeakyObject leakyObject=new LeakyObject();
            var leaky = new WeakReference(leakyObject);
            leakyObject.RunMethod();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Assert.IsTrue(leaky.IsAlive);
        }
         [Test]
        public void NewNonLeaky_Nulled_IsGarbageCollected()
        {
            var nonLeakyObject=new NonLeakyObject();
            var nonLeakyRef=new WeakReference(nonLeakyObject);
            nonLeakyObject.RunMethod();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Assert.IsFalse(nonLeakyRef.IsAlive);
        }
    }
}