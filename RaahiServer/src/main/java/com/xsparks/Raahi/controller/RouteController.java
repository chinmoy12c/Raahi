package com.xsparks.Raahi.controller;

import java.util.ArrayList;
import java.util.HashMap;

import com.xsparks.Raahi.dao.EdgeRepo;
import com.xsparks.Raahi.dao.LocationRepo;
import com.xsparks.Raahi.dao.POIRepo;
import com.xsparks.Raahi.dao.PathNodeRepo;
import com.xsparks.Raahi.models.Edge;
import com.xsparks.Raahi.models.Location;
import com.xsparks.Raahi.models.POI;
import com.xsparks.Raahi.models.PathNode;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import net.minidev.json.JSONObject;

@RestController
public class RouteController {

    @Autowired
    PathNodeRepo pathNodeRepo;
    @Autowired
    LocationRepo locationRepo;
    @Autowired
    EdgeRepo edgeRepo;
    @Autowired
    POIRepo poiRepo;

    @PostMapping("/saveRoute")
    void saveRoute(@RequestBody HashMap<String, Object> request) {
        ArrayList<String> nodes = (ArrayList<String>) request.get("nodes");
        String locationName = (String) request.get("locationName");
        String poiName = (String) request.get("poiName");
        Location location = locationRepo.getByLocationName(locationName);
        if (location != null) {
            return;
        }
        location = new Location();
        location.setLocationName(locationName);
        Location createdLocation = locationRepo.save(location);
        ArrayList<PathNode> createdNodes = new ArrayList<>();
        for (String node : nodes) {
            PathNode pathNode = new PathNode();
            pathNode.setLocation(createdLocation);
            pathNode.setVector(node);
            PathNode createdNode = pathNodeRepo.save(pathNode);
            if (createdNodes.size() > 0) {
                Edge edge = new Edge();
                edge.setFrom(createdNodes.get(createdNodes.size() - 1));
                edge.setTo(createdNode);
                edgeRepo.save(edge);
            }
            createdNodes.add(createdNode);
        }
        POI routePoi = new POI();
        routePoi.setNode(createdNodes.get(createdNodes.size() - 1));
        routePoi.setPoiName(poiName);
        poiRepo.save(routePoi);
    }

    @PostMapping("/getLocation")
    JSONObject getLocation(@RequestBody HashMap<String, String> request) {
        Location location = locationRepo.getByLocationName(request.get("locationName"));
        PathNode[] nodes = pathNodeRepo.getByLocation(location);
        Edge[] edges = edgeRepo.getByToIn(nodes);
        POI[] pois = poiRepo.getByPathNodeIn(nodes);
        JSONObject respone = new JSONObject();
        respone.put("routeData", edges);
        respone.put("pois", pois);
        return respone;
    }
}
