# Test conventions

Here are captured conventions around tests used in this project like naming and structure

## Naming convention of tests

Tests are named based on following pattern:

```csharp
public void Test<TestedMethod><Positive|Negative><Context>
```

Notes:
- TestedMethod - name of tested method
- Positive - expected behaviour of tested code when no issues
- Negative - expected behaviour of tested code with issues (thrown exception, returned null, ...)
- Context - optional part of test name for case that there are multiple scenarios for positive or negative type

Example:

Example from [CycleServiceTests](../UnitTests/CycleServiceTests.cs)
```csharp
public void TestActivatePositiveInactiveCycle
```
is test expected behaviour without issues for Activate method of CycleService when cycle in inactive state is being passed

## Structure of tests

Parts of tests are separated to blocks which start with descriptive comment

Comment type:
- Init - initialization part of test, setting up mocks and similarly
- Preverify - assertions before call of tested method, with goal to emphasize change of state in test
- Test - call of tested method
- Verify - assertions after call of tested method to verify expected behaviour and mock interactions
- Test & Verify - joined Test and Verify blocks for case of testing exception throwing (call of method is argument of Assert.ThrowsException)
