#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NSubstitute;
using NUnit.Framework;
using Player;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerMovementTest
{
    private PlayerMovement playerMovement;
    
    private IGroundedDetector groundDetector;
    private IInputGatherer inputGatherer;
    
    private static string ScenePath // path to the test scene, if this test breaks, this scene load might be silently failing
    {
        get
        {
            var slash = Path.DirectorySeparatorChar;
            var scenePath =
                string.Format("Assets{0}Tests{0}PlayModeTests{0}PlayerMovement{0}Scenes{0}PlayerMovementTest.unity",slash);
            return scenePath;
        }
    }
    
   
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        LoadScene();

        yield return null; // wait for the scene to load

        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();

        Assert.NotNull(playerMovement);
        
        groundDetector = Substitute.For<IGroundedDetector>();
        inputGatherer = Substitute.For<IInputGatherer>();
        playerMovement.SetDependencies(inputGatherer, groundDetector);
    }


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator MovesWhenInputIsGivenAndIsGrounded()
    {
        groundDetector.CanWalk.Returns(true);
       // inputGatherer.ReadMovementInput().Returns(new Vector2 (1, 0));

        var initialPos = playerMovement.transform.position;
        
        yield return new WaitForFixedUpdate();
        
        var newPos = playerMovement.transform.position;
        
        Assert.Greater(newPos.x, initialPos.x);
    }
    
    private static void LoadScene()
    {
#if UNITY_EDITOR // unity gets mad about using editor only functions otherwise
            EditorSceneManager.LoadSceneInPlayMode(ScenePath, new LoadSceneParameters(LoadSceneMode.Single));
#endif
    }
}
#endif