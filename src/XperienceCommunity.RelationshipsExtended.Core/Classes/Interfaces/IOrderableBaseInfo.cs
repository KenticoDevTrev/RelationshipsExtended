namespace RelationshipsExtended.Interfaces
{
    /// <summary>
    /// Orderable Interface used for the Edit Bindings with Node + Orderable Support, ____Info class implement these for the UI to work.
    /// </summary>
    public interface IOrderableBaseInfo
    {
        /// <summary>
        /// Sets the object's current position to this position.
        /// use this.Generalized.SetObjectOrder(Order); this.SetObject(); in implimenation
        /// </summary>
        /// <param name="Order">The Order</param>
        void SetObjectOrder(int Order);
        /// <summary>
        /// Changes the object's position based on relative value, if positive it moves it up, if negative it moves it down.
        /// use this.Generalized.SetObjectOrder(PositionChange, true); this.SetObject(); in implimentation
        /// </summary>
        /// <param name="PositionChange">The Position Change relative to itself.</param>
        void SetObjectOrderRelative(int PositionChange);
        /// <summary>
        /// Moves the object up one position
        /// use this.Generalized.MoveObjectUp(); this.SetObject(); in implimentation
        /// </summary>
        void MoveObjectUp();
        /// <summary>
        /// Moves the object down one position
        /// use this.Generalized.MoveObjectDown(); this.SetObject(); in implimentation
        /// </summary>
        void MoveObjectDown();
    }
}
