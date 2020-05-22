using UnityEngine;

namespace ProceduralToolkit.Samples
{
    public static class Stretchers
    {
        public static MeshDraft XStretchers(Vector3[] legCenters, float legWidth, float legHeight)
        {
            var draft = new MeshDraft();
            legWidth = RandomE.Range(legWidth/2, legWidth*3/4, 2);
            draft.Add(ChairGenerator.BeamDraft(legCenters[0], legCenters[2], legWidth));
            draft.Add(ChairGenerator.BeamDraft(legCenters[1], legCenters[3], legWidth));
            draft.Move(Vector3.up*RandomE.Range(legHeight/4, legHeight/2, 2));
            return draft;
        }

        public static MeshDraft HStretchers(Vector3[] legCenters, float legWidth, float legHeight)
        {
            var draft = new MeshDraft();
            legWidth = RandomE.Range(legWidth/2, legWidth, 3);
            draft.Add(ChairGenerator.BeamDraft(legCenters[0], legCenters[3], legWidth));
            draft.Add(ChairGenerator.BeamDraft(legCenters[1], legCenters[2], legWidth));
            Vector3 leftCenter = (legCenters[3] + legCenters[0])/2;
            Vector3 rightCenter = (legCenters[2] + legCenters[1])/2;
            draft.Add(ChairGenerator.BeamDraft(leftCenter, rightCenter, legWidth));
            draft.Move(Vector3.up*RandomE.Range(legHeight/4, legHeight*3/4, 3));
            return draft;
        }

        public static MeshDraft BoxStretchers(Vector3[] legCenters, float legWidth, float legHeight)
        {
            var draft = new MeshDraft();
            legWidth = RandomE.Range(legWidth/2, legWidth, 3);
            MeshDraft stretcher0 = ChairGenerator.BeamDraft(legCenters[0], legCenters[1], legWidth);
            stretcher0.Add(ChairGenerator.BeamDraft(legCenters[2], legCenters[3], legWidth));
            stretcher0.Move(Vector3.up*RandomE.Range(legHeight/4, legHeight*3/4, 3));
            MeshDraft stretcher1 = ChairGenerator.BeamDraft(legCenters[0], legCenters[3], legWidth);
            stretcher1.Add(ChairGenerator.BeamDraft(legCenters[1], legCenters[2], legWidth));
            stretcher1.Move(Vector3.up*RandomE.Range(legHeight/4, legHeight*3/4, 3));
            draft.Add(stretcher0);
            draft.Add(stretcher1);
            return draft;
        }
    }
}
