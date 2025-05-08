using NUnit.Framework;
using Reqnroll;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]