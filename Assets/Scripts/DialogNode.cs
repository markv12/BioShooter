using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogNode {
    public string text;
    public DialogOption[] options;
    public SpeshI speshi = SpeshI.None;
}

public class DialogOption {
    public string text;
    public DialogNode dest;
}

public enum SpeshI { //Special Instruction
    None,
    MakeFriendly
}