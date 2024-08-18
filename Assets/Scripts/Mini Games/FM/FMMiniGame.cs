using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Represents the "Fortress Maze" MiniGame
/// </summary>
public class FMMiniGame : MiniGame
{
    [Header("Fortress Maze")]
    [SerializeField] private GameObject prefabRoom;
    [SerializeField] private Texture2D mouseIcon;
    [SerializeField] private float cameraSpeed = 3;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float cameraMoveZ = 20.40287f;
    [SerializeField] private float cameraOffsetX = 1.936262f;
    private GameObject currentRoom;
    private GameObject nextRoom;
    private bool movingCamera;
    private Vector2 nextPosition;
    private Transform cameraTransform;

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        movingCamera = false;
        cameraTransform = Camera.main.transform;
        currentRoom = Instantiate(prefabRoom, prefabRoom.transform.position, Quaternion.identity);
    }

    protected override void MiniGameUpdate()
    {
        if (movingCamera)
        {
            if (cameraTransform.position.x != nextPosition.x)
            {

                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position,
                new Vector3(nextPosition.x, cameraTransform.position.y, cameraTransform.position.z), Time.deltaTime * cameraSpeed);
            }
            else if (cameraTransform.position.z != nextPosition.y)
            {
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position,
                new Vector3(cameraTransform.position.x, cameraTransform.position.y, nextPosition.y), Time.deltaTime * cameraSpeed);
            }
            else
            {
                movingCamera = false;
                Destroy(currentRoom);
                currentRoom = nextRoom;
            }
        }
        else
        {
            RaycastHit hit;
            Transform current = null;


            if (!EventSystem.current.IsPointerOverGameObject() &&
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500, mask))
            {
                current = hit.transform;
                Cursor.SetCursor(mouseIcon, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }

            if (current != null && Input.GetMouseButtonDown(0))
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

                int idxChosen = int.Parse(current.name);

                if (Random.Range(0, 4) == 0)
                {
                    EndMiniGame();
                    return;
                }

                nextRoom = Instantiate(prefabRoom, cameraTransform.GetChild(idxChosen).position, Quaternion.identity);
                current.GetComponent<Animator>().SetTrigger("Open");

                nextPosition = new Vector2(cameraTransform.GetChild(idxChosen).position.x + cameraOffsetX, cameraTransform.position.z + cameraMoveZ);

                movingCamera = true;
            }
        }
    }
}
