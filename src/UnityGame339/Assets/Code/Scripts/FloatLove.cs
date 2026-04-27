using UnityEngine;

public class FloatLove : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;

        [SerializeField] private TextAsset CantDateFile;
                [SerializeField] private TextAsset LoveFile;
                [SerializeField] private TextAsset PoorFile;
                [SerializeField] private TextAsset NewFile;


    public float speed = 1;

private string[] linesCant;
private string[] linesLove;
private string[] linesPoor;
private string[] linesNew;

public int Loveis1NoLoveis0 = 0;

    void Awake() {
        // Split the file text by new line characters
        linesCant = CantDateFile.text.Split('\n');
        linesLove = LoveFile.text.Split('\n');
        linesPoor = PoorFile.text.Split('\n');
        linesNew = NewFile.text.Split('\n');

        // Choose a random line
        //string randomLine = linesCant[Random.Range(0, linesCant.Length)];
        
    }

    
    void Update()
    {
        gameObject.transform.Translate(0, speed * Time.deltaTime, 0);
    }

    public void SetTextCant()
    {
        textMesh.text = linesCant[Random.Range(0, linesCant.Length)];
    }

    public void SetTextLove()
    {
        textMesh.text = linesLove[Random.Range(0, linesLove.Length)];
    }

    public void SetTextPoor()
    {
        textMesh.text = linesPoor[Random.Range(0, linesPoor.Length)];
    }

    public void SetTextNew()
    {
        textMesh.text = linesNew[Random.Range(0, linesNew.Length)];
    }
    

}
