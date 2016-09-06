using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public static class Backs
    {
        public static MeshDraft Back0(Vector3 center, float width, float length, float height)
        {
            var draft = MeshDraft.Hexahedron(width, length, height);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }

        public static MeshDraft Back1(Vector3 center, float width, float length, float height)
        {
            var draft = new MeshDraft();
            int plankCount = Random.Range(1, 5);
            float plankStep = height/plankCount;
            float plankHeight = plankStep*Random.Range(0.3f, 0.8f);
            float plankWidth = width - length*2;

            float offeset = 0;
            if (plankCount > 1)
            {
                offeset = RandomE.Range(0, (plankStep - plankHeight)/2, 3);
            }
            Vector3 startPosition = Vector3.up*(-height/2 + plankStep - plankHeight/2 - offeset);
            for (int i = 0; i < plankCount; i++)
            {
                var plank = MeshDraft.Hexahedron(plankWidth, length, plankHeight);
                plank.Move(startPosition + Vector3.up*i*plankStep);
                draft.Add(plank);
            }
            var rod = MeshDraft.Hexahedron(length, length, height);
            rod.Move(Vector3.left*(width/2 - length/2));
            draft.Add(rod);
            rod.Move(Vector3.right*(width - length));
            draft.Add(rod);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }

        public static MeshDraft RodBack(Vector3 center, float width, float length, float height)
        {
            var draft = new MeshDraft();
            int rodCount = Random.Range(1, 5);
            float maxWidth = (width - length*2)/rodCount;
            float rodWidth = RandomE.Range(maxWidth/4, maxWidth*3/4, 3);
            float interval = (width - length*2 - rodWidth*rodCount)/(rodCount + 1);
            float upperRodHeight = Mathf.Min(height*3/4, length*Random.Range(1, 4));
            float rodHeight = height - upperRodHeight;

            var leftRod = MeshDraft.Hexahedron(length, length, rodHeight);
            leftRod.Move(Vector3.left*(width - length)/2 + Vector3.down*upperRodHeight/2);
            draft.Add(leftRod);
            leftRod.Move(Vector3.right*(width - length));
            draft.Add(leftRod);

            Vector3 startPosition = Vector3.left*(width/2 - length - interval - rodWidth/2) +
                                    Vector3.down*upperRodHeight/2;
            for (int i = 0; i < rodCount; i++)
            {
                var rod = MeshDraft.Hexahedron(rodWidth, length, rodHeight);
                rod.Move(startPosition + Vector3.right*i*(rodWidth + interval));
                draft.Add(rod);
            }
            var upperRod = MeshDraft.Hexahedron(width, length, upperRodHeight);
            upperRod.Move(Vector3.up*rodHeight/2);
            draft.Add(upperRod);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }
    }
}