package com.cs110.team10.whiskrs;

import android.content.Intent;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.AdapterView.OnItemClickListener;

import com.goebl.david.Request;
import com.goebl.david.Response;
import com.goebl.david.Webb;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.net.CookieManager;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;

import android.widget.BaseAdapter;
import android.widget.ArrayAdapter;

public class IngredientsInput extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ingredients_input);

        Button search = (Button)findViewById(R.id.searchIngredients);
        search.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                final EditText in = (EditText) findViewById(R.id.editText);
                final String text = in.getText().toString();
                clearInput(in);
                new Thread(new Runnable() {
                    public void run() {
                        new SCRequest().execute(text);
                    }
                }).start();
            }
        });
        final ListView lv = (ListView)findViewById(R.id.output);
        lv.setOnItemClickListener(new OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> arg0, View arg1, int position, long arg3) {
                String text = ((TextView)arg1).getText().toString();
                String url = text.split(": ")[1];
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(url));
                startActivity(browserIntent);
            }
        });
    }

    protected void showResults(JSONObject res){
        final ListView out = (ListView)findViewById(R.id.output);
        ArrayList<String> arrayList = new ArrayList<String>();
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(getApplicationContext(), android.R.layout.simple_spinner_item, arrayList);
        out.clearChoices();
        out.setAdapter(adapter);
        SuperCookResult scr = new SuperCookResult(res);
        for(int i=0;i<scr.results.size();i++){
            arrayList.add(scr.results.get(i).title + ": " + scr.results.get(i).url);
        }
    }

    protected class SCRequest extends AsyncTask<String,Void,JSONObject> {
        protected JSONObject doInBackground(String... ingredients) {
            return SuperCookRequest.getRecipes(ingredients[0]);
        }

        protected void onPostExecute(JSONObject result) {
            showResults(result);
        }
    }

    private void clearInput(EditText in){
        in.setText("");
    }
}
