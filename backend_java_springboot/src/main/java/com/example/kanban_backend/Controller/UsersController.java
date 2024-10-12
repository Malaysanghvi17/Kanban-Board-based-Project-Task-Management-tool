package com.example.kanban_backend.Controller;
import com.example.kanban_backend.Model.Users;
import com.example.kanban_backend.Repository.UsersRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Optional;

@RestController
@RequestMapping("/api/users")
public class UsersController {

    @Autowired
    private UsersRepository usersRepository;

    @GetMapping("/{uid}")
    public ResponseEntity<Users> getUser(@PathVariable Integer uid) {
        Users user = usersRepository.findById(uid).orElse(null);
        if (user == null) return ResponseEntity.notFound().build();
        return ResponseEntity.ok(user);
    }

    @PostMapping
    public ResponseEntity<Users> createUser(@RequestBody Users newUser) {
        Users savedUser = usersRepository.save(newUser);
        return ResponseEntity.created(null).body(savedUser);
    }

    @PutMapping("/{uid}")
    public ResponseEntity<Void> updateUser(@PathVariable Integer uid, @RequestBody Users updatedUser) {
        Users user = usersRepository.findById(uid).orElse(null);
        if (user == null) return ResponseEntity.notFound().build();

        user.setName(updatedUser.getName());
        user.setEmail(updatedUser.getEmail());
        user.setCreatedAt(updatedUser.getCreatedAt());
        usersRepository.save(user);
        return ResponseEntity.noContent().build();
    }

    @DeleteMapping("/{uid}")
    public ResponseEntity<Void> deleteUser(@PathVariable Integer uid) {
        Users user = usersRepository.findById(uid).orElse(null);
        if (user == null) return ResponseEntity.notFound().build();

        usersRepository.delete(user);
        return ResponseEntity.noContent().build();
    }
}

