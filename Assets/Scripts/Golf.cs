using UnityEngine;

public class Golf : MonoBehaviour
{
    [SerializeField]
    private Hole hole;

    private Vector2 targetPosition;

    void Start()
    {
        if (hole != null)
        {
            // Choose either random or specific coordinate
            Hole.Coordinate chosenCoordinate = hole.GetRandomCoordinate();
            // Hole.Coordinate chosenCoordinate = hole.GetCoordinateByIndex(12); // Example of choosing a specific coordinate by index

            if (chosenCoordinate != null)
            {
                targetPosition = new Vector2(chosenCoordinate.x, chosenCoordinate.y);
                MoveGolfBall();
            }
        }
    }

    void MoveGolfBall()
    {
        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.y);
        Debug.Log("Golf ball moved to: (" + targetPosition.x + ", " + targetPosition.y + ")");
    }

    // Method to set a specific coordinate externally
    public void SetTargetPosition(Hole.Coordinate coordinate)
    {
        if (coordinate != null)
        {
            targetPosition = new Vector2(coordinate.x, coordinate.y);
            MoveGolfBall();
        }
    }
}