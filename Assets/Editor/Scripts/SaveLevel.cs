using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevLabirinth
{
    public class SaveLevel
    {
        public List<BlockObject> GetBlocks()
        {
            List<BlockObject> objects = new List<BlockObject>();
            GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");

            foreach (var item in allBlocks)
            {
                BlockObject blockObject = new BlockObject();
                blockObject.Position = item.gameObject.transform.position;

                if (item.TryGetComponent(out Block block))
                {
                    blockObject.Block = block.BlockData;
                }

                if (item.TryGetComponent(out OtherBlock otherBlock))
                {
                    blockObject.Block = otherBlock.BlockData;
                }

                objects.Add(blockObject);
            }

            return objects;
        }
    }
}
