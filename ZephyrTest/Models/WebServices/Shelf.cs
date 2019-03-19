using System;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ZephyrTest.Models.WebServices
{
    class Shelf:IWebServiceEntity, IEquatable<Shelf>
    {
        [JsonProperty(PropertyName = "rcNumber")]
        public int RcNumber { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        public Shelf()
        {
            this.RcNumber = 123456;
            this.Id = Guid.NewGuid().ToString();
            this.Name = "ATS" + new Random().Next(100);
            this.Description = "Auto Test Shelf";
        }

        public void AssertEquals(IWebServiceEntity other)
        {
            Shelf otherShelf = (Shelf) other;
            Assert.AreEqual(this.Id, otherShelf.Id);
            Assert.AreEqual(this.Name, otherShelf.Name);
            Assert.AreEqual(this.Description, otherShelf.Description);
        }

        public IWebServiceEntity Clone() => new Shelf
        {
            RcNumber = this.RcNumber,
            Id = Guid.NewGuid().ToString(),
            Name = this.Name,
            Description = this.Description
        };

        public IWebServiceEntity Invalidate()
        {
            this.Description += "^!%@$#$^@#&$";
            return this;
        }

        public IWebServiceEntity Update()
        {
            this.Description += "Updated";
            return this;
        }

        public string GetName() => this.Name;

        public string GetId() => this.Id;

        override public string ToString() => string.Format("{1}{0}{2}{0}{3}{0}{4}", Environment.NewLine, RcNumber,Id,Name,Description);

        public bool Equals(Shelf obj) => this.Name == obj.Name;

    }
}