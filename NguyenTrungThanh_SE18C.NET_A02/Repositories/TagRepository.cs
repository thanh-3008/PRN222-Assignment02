using BusinessObjects;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly FUNewsManagementDbContext _context;
        public TagRepository(FUNewsManagementDbContext context) { _context = context; }

        public List<Tag> GetAllTags()
        {
            return _context.Tags.ToList();
        }

        public Tag GetOrCreateTagByName(string tagName)
        {
            var normalizedTagName = tagName.Trim();
            var existingTag = _context.Tags.FirstOrDefault(t => t.TagName.ToUpper() == normalizedTagName.ToUpper());

            if (existingTag != null)
            {
                return existingTag; 
            }
            else
            {
                var newTag = new Tag { TagName = normalizedTagName };
                _context.Tags.Add(newTag);
                _context.SaveChanges();
                return newTag;
            }
        }
    }
}