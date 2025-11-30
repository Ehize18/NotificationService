namespace NotificationService.Shared.Entities
{
	/// <summary>
	/// Base class for entities.
	/// </summary>
	public abstract class BaseEntity
	{
		/// <summary>
		/// Id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Create date.
		/// </summary>
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Update date.
		/// </summary>
		public DateTime? UpdatedAt { get; set; }

		/// <summary>
		/// Creator id.
		/// </summary>
		public required Guid CreatedBy { get; set; }

		/// <summary>
		/// Updated id.
		/// </summary>
		public Guid? UpdatedBy { get; set; }
	}
}
