using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using HomeworkTask.Models;

namespace HomeworkTask.Controllers
{
    public class TaskController : ApiController
    {
        [Route("Tasks/EfficientPaths")]
        [HttpGet]
        public HttpResponseMessage GetMostEfficientPaths(string jumps)
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            var pathsToReturn = new List<List<Node>>();
            List<Node> taskNodes = jumps.Split(',').Select((num, ind) => new Node(int.Parse(num), ind)).ToList();
            List<List<Node>> paths = new List<List<Node>>();
            AllRoutes(taskNodes, 0, taskNodes.Count - 1, paths);
            if (paths.Count > 0)
            {
                var orderedPaths = paths.OrderBy(path => path.Count).ToList();
                pathsToReturn = orderedPaths.Where(orderedPath => orderedPath.Count == orderedPaths[0].Count).ToList();
            }
            resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new ObjectContent<List<List<Node>>>(pathsToReturn, new JsonMediaTypeFormatter());
            return resp;
        }

        public List<List<Node>> AllRoutes(List<Node> taskNodes, int currentIndex, int targetIndex, List<List<Node>> paths, List<Node> currentPath = null)
        {
            currentPath = currentPath == null ? new List<Node>() : currentPath;
            List<Node> tempCurrentPath = currentPath.Select(node => (Node)node.Clone()).ToList();
            var currentNode = (Node)taskNodes[currentIndex].Clone();
            if (currentIndex == targetIndex)
            {
                tempCurrentPath.Add(currentNode);
                paths.Add(tempCurrentPath);
                return paths;
            }
            if (taskNodes[currentIndex].Value > 0)
            {
                tempCurrentPath.Add(currentNode);

                for (int i = currentIndex + 1; i <= targetIndex && i <= currentNode.Value + currentIndex; i++)
                {
                    paths = AllRoutes(taskNodes, i, targetIndex, paths, tempCurrentPath);
                }
            }

            return paths;
        }
    }
}
