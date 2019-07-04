namespace CprPrototype.Model
{
    /// <summary>
    /// The different types of drugs available
    /// </summary>
    public enum DrugType
    {
        Epinephrine,
        Amiodarone,
        Bicarbonate,
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
        Hypoxia,
        Hypovolaemia,
        Hyperkalaemia,
        Hypothermia
    }

    /// <summary>
    /// The special mnemonics for the 4 T's
    /// </summary>
    public enum SpecialCasesT
    {
        Thrombosis,
        Tension,
        Tamponade,
        Toxins
    }
}
