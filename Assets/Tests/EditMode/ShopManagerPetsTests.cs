using UnityEngine;
using NUnit.Framework;

public class CompanionManagerTest
{
    private CompanionManager companionManager;

    [SetUp]
    public void SetUp()
    {
        // Create a GameObject to attach the CompanionManager to
        GameObject companionManagerObject = new GameObject();
        companionManager = companionManagerObject.AddComponent<CompanionManager>();

        // Initialize the CompanionManager
        companionManager.Start();
    }

    [Test]
    public void TestCompanionInitialization()
    {
         // Check that the companions list has been initialized with the correct number of companions
        Assert.AreEqual(15, companionManager.companions.Count, "Companion list should contain 15 companions.");

        // Check that the first companion is initialized correctly
        var firstCompanion = companionManager.GetCompanionById(0);
        Assert.IsNotNull(firstCompanion, "First companion should not be null.");
        Assert.AreEqual("Alien", firstCompanion.PetName, "First companion name should be 'Alien'.");
        Assert.AreEqual(0, firstCompanion.CompanionID, "First companion ID should be 0.");
        Assert.AreEqual(50, firstCompanion.SatisfactionLevel, "First companion's initial satisfaction should be 50.");
        Assert.AreEqual(1, firstCompanion.Level, "First companion's initial level should be 1.");
        Assert.IsFalse(firstCompanion.IsBought, "First companion should not be bought initially.");
    }

    [Test]
    public void TestSetCompanionBought()
    {
        // Test setting a companion as bought
        companionManager.SetCompanionBought(1); // Buy the second companion (Berry)

        var boughtCompanion = companionManager.GetCompanionById(1);
        Assert.IsTrue(boughtCompanion.IsBought, "Companion Berry should be marked as bought.");
    }

    [Test]
    public void TestIsCompanionBought()
    {
        // Check that a companion that hasn't been bought returns false
        Assert.IsFalse(companionManager.IsCompanionBought(0), "Companion 0 (Alien) should not be bought.");

        // Buy companion 0
        companionManager.SetCompanionBought(0);

        Assert.IsTrue(companionManager.IsCompanionBought(0), "Companion 0 (Alien) should now be marked as bought.");
    }

    [TearDown]
    public void TearDown()
    {
        // Cleanup the GameObject after each test
        Object.DestroyImmediate(companionManager.gameObject); // Use DestroyImmediate for edit mode
    }
}
