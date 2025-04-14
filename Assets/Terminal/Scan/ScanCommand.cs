using UnityEngine;

public class ScanCommand : ShellCommand
{
    [SerializeField] private Rover rover;
    [SerializeField] private float scanRadius;
    [SerializeField] private LayerMask layerMask;
    public override void Execute(string[] args)
    {
        if (args.Length > 2)
        {
            PrintHelp();
            return;
        }

        string specificName = null;
        if (args.Length == 2)
        {
            specificName = args[1];
        }

        var hits = Physics.OverlapSphere(
            rover.transform.position,
            scanRadius,
            layerMask.value,
            QueryTriggerInteraction.Collide
        );
        bool found = false;
        foreach (var hit in hits)
        {
            var obj = hit.GetComponent<Scanable>();
            if (obj != null)
            {
                if (specificName == null)
                {
                    Terminal.Println($"Found: {obj.Name}");
                    found = true;
                }
                else if (obj.Name == specificName)
                {
                    Terminal.Println($"Found: {obj.Name}");
                    Terminal.Println($"Description: {obj.Description}");
                    found = true;
                }
            }
        }

        if (found) return;
        Terminal.Println(specificName != null ? $"No object found with name: {specificName}" : "No objects found.");
    }
}
