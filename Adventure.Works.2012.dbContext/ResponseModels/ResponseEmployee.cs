using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Adventure.Works._2012.dbContext.ResponseModels
{
    public class ResponseEmployee
    {
      
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }
        [Required]
        [StringLength(10)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string Title { get; set; }

        [StringLength(60)]
        public string Address { get; set; }
        [StringLength(15)]
        public string City { get; set; }
        [StringLength(15)]
        public string Region { get; set; }
        [StringLength(10)]
        public string PostalCode { get; set; }
        [StringLength(15)]
        public string Country { get; set; }
        [StringLength(24)]
        public string HomePhone { get; set; }

    
        }

        [ProtoContract]
        public class ResponseEmployeeProtobuf
        {
            [ProtoMember(1)]
            public int EmployeeId { get; set; }

            [ProtoMember(2)]
            public string LastName { get; set; }
            [ProtoMember(3)]
            public string FirstName { get; set; }

            [ProtoMember(4)]
            public string Title { get; set; }

            [ProtoMember(5)]
            public string Address { get; set; }
            [ProtoMember(6)]
            public string City { get; set; }
            [ProtoMember(7)]
            public string Region { get; set; }
            [ProtoMember(8)]
            public string PostalCode { get; set; }
            [ProtoMember(9)]
            public string Country { get; set; }
            [ProtoMember(10)]
            public string HomePhone { get; set; }


        }
    
}

