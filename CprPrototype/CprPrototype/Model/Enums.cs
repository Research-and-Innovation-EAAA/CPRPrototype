namespace CprPrototype.Model
{
    /// <summary>
    /// The different types of drugs available
    /// </summary>
    public enum DrugType
    {
        Adrenalin,
        Amiodaron,
        Bikarbonat,
        Calcium,
        Magnesium
    }

    /// <summary>
    /// The states the patients heartbeat can be in
    /// </summary>
    public enum RythmStyle
    {
        Shockable,
        NonShockable
    }

    /// <summary>
    /// The special mnemonics for the 4 H's
    /// </summary>
    public enum SpecialCasesH
    {
        Hypoxi,
        Hypovolæmi,
        Hyperkalæmi,
        Hypotermi
    }

    /// <summary>
    /// The special mnemonics for the 4 T's
    /// </summary>
    public enum SpecialCasesT
    {
        Tamponade,
        Trykneumothorax,
        Trombose,
        Toksisk
    }
}
