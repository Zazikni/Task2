namespace Events
{
    #region enums
    enum SocialRole
    {
        Buyer,
        Strangers,
        Citizens,
        NS
    }
    enum PronounsGroups
    {
        FirstPers,
        SecondPers,
        ThirdPers,
        NS
    }
    #endregion
    /// <summary>
    /// Класс реализующий социальную группу.
    /// </summary>
    internal class SocietyGroup: SocietyElement
    {
        #region fields
        string _name;
        public SocialRole SocialRole { get; set; }
        public PronounsGroups Prn { get; set; }
        public override string Name 
        {
            get
            {
                if (SocialRole != SocialRole.NS)
                    switch (SocialRole)
                    {
                        case SocialRole.Buyer:
                            {
                                return "Покупатели";
                            }
                        case SocialRole.Strangers:
                            {
                                return "Прохожие";
                            }
                        case SocialRole.Citizens:
                            {
                                return "Горожане";
                            }
                        default:
                            {
                                return _name;
                            }
                    }
                else if(Prn != PronounsGroups.NS)
                    switch (Prn)
                    {
                        case PronounsGroups.FirstPers:
                            {
                                return "МЫ";
                            }
                        case PronounsGroups.SecondPers:
                            {
                                return "ВЫ";
                            }
                        case PronounsGroups.ThirdPers:
                            {
                                return "Они";
                            }
                        default:
                            {
                                return _name;
                            }
                    }
                else
                {
                    return _name;
                }
                
            }
        }
        #endregion
        #region constructors
        public SocietyGroup(params Person[] members ) 
        {
            foreach( Person member in members )
            {
                if (_name == null)
                { 
                    _name = member.Name;
                }
                else
                {
                    _name += $" и {member.Name}";
                }
                
            }
            SetDefault();
        }
        public SocietyGroup(string name)
        {
            _name=name;
            SetDefault();


        }
        #endregion
        #region methods
        public void SetDefault()
        {
            SocialRole = SocialRole.NS;
            Prn = PronounsGroups.NS;
        }
        #endregion
    }
}
