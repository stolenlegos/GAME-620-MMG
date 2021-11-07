using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem {
  ItemID ID { get; set; }
}

public enum ItemID {
  sword,
  jewels,
  shield,
  bread,
  corn,
  book
}
