using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlBrewer.Uwp.SqLiteSample.Models
{
    /// <summary>
    /// Represents a person.
    /// </summary>
    internal class Person
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [MaxLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the day of birth.
        /// </summary>
        public DateTimeOffset DayOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the picture.
        /// </summary>
        /// <remarks>Is a blob in the database.</remarks>
        public byte[] Picture { get; set; }
    }

}
