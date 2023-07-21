using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FocusSkill : ScriptableObject {
    public int maxCombo = 15;

    public virtual void Init() { }

    public virtual void BeforeSkill() { }
    protected abstract void Invoke(int combo);

    public void UseSkill(int combo) {
        Invoke(Mathf.Min(combo, maxCombo));
    }
}
