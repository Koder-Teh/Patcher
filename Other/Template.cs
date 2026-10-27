using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Patcher
{
    [JsonSerializable(typeof(Template))]
    internal partial class SourceGenerationContext : JsonSerializerContext { }

    internal class Template
    {
        [JsonPropertyName("length_buffer_mask")]
        public int buffer_length { get; set; }

        [JsonPropertyName("patchs")]
        public List<Patchs> patchs { get; set; }
    }

    internal class Patchs
    {
        [JsonPropertyName("file_offset")]
        public string offset { get; set; }

        [JsonPropertyName("mask_byte")]
        public string mask { get; set; }

        [JsonPropertyName("restore_byte")]
        public string restore { get; set; }

        [JsonPropertyName("patch_byte")]
        public string patch { get; set; }
    }
}
